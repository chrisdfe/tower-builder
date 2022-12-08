using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Furnitures
{
    using FurnitureStateSlice = ListStateSlice<FurnitureList, Furniture, State.Events>;

    [Serializable]
    public class State : FurnitureStateSlice
    {
        public class Input { }

        public new class Events : FurnitureStateSlice.Events
        {
            public FurnitureStateSlice.Events.ListEvent onItemBuilt;
        }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.state = state;
            }

            public FurnitureList FindFurnitureByRoom(Room room) =>
                new FurnitureList(
                    state.list.items.FindAll(furniture =>
                        appState.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]) == room
                    )
                );

            public Furniture FindFurnitureAtCell(CellCoordinates cellCoordinates)
            {
                return state.list.FindFurnitureAtCell(cellCoordinates);
            }

            public FurnitureList FindFurnitureInBlocks(CellCoordinatesBlockList cellCoordinatesBlockList)
            {
                List<Furniture> furnitureList = new List<Furniture>();

                foreach (CellCoordinatesBlock block in cellCoordinatesBlockList.items)
                {
                    foreach (CellCoordinates cellCoordinates in block.items)
                    {
                        Furniture furnitureAtCell = FindFurnitureAtCell(cellCoordinates);
                        if (furnitureAtCell != null)
                        {
                            furnitureList.Add(furnitureAtCell);
                        }
                    }
                }

                return new FurnitureList(furnitureList);
            }
        }

        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
            Setup();
        }

        public override void Setup()
        {
            appState.Rooms.events.onItemsAdded += OnRoomsAdded;
            appState.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            appState.Rooms.events.onItemsRemoved += OnRoomsRemoved;

            appState.Rooms.events.onRoomBlocksAdded += OnRoomBlocksAdded;
            appState.Rooms.events.onRoomBlocksRemoved += OnRoomBlocksRemoved;
        }

        public override void Teardown()
        {
            appState.Rooms.events.onItemsAdded -= OnRoomsAdded;
            appState.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            appState.Rooms.events.onItemsRemoved -= OnRoomsRemoved;

            appState.Rooms.events.onRoomBlocksAdded -= OnRoomBlocksAdded;
            appState.Rooms.events.onRoomBlocksRemoved -= OnRoomBlocksRemoved;
        }

        public void Build(Furniture furniture)
        {
            furniture.validator.Validate(appState);

            if (!furniture.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (EntityValidationError validationError in furniture.validator.errors.items)
                {
                    appState.Notifications.Add(new Notification(validationError.message));
                }
                return;
            }

            furniture.OnBuild();
            // furniture.room = appState.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]);

            events.onItemBuilt?.Invoke(new FurnitureList(furniture));
        }

        public override void Remove(Furniture furniture)
        {
            base.Remove(furniture);
            furniture.OnDestroy();
        }

        /* 
            Event Handlers
        */
        void OnRoomsAdded(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                FurnitureList roomFurnitures = room.furnitureBuilder.BuildFurniture(room.isInBlueprintMode);
                Add(roomFurnitures);
            });
        }

        void OnRoomsBuilt(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                FurnitureList roomFurnitures = queries.FindFurnitureByRoom(room);

                FurnitureList blueprintFurnitures = new FurnitureList(
                    roomFurnitures.items.FindAll(roomFurniture => roomFurniture.isInBlueprintMode == true).ToList()
                );

                if (blueprintFurnitures.Count > 0)
                {
                    blueprintFurnitures.ForEach(furniture =>
                    {
                        Build(furniture);
                    });

                    events.onItemBuilt?.Invoke(blueprintFurnitures);
                }
            });
        }

        void OnRoomsRemoved(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                FurnitureList furnituresToRemove = queries.FindFurnitureByRoom(room);
                Remove(furnituresToRemove);
            });
        }

        void OnRoomBlocksAdded(Room room, CellCoordinatesBlockList roomBlocks)
        {
            // Recalculate furniture in room
        }

        void OnRoomBlocksRemoved(Room room, CellCoordinatesBlockList roomBlocks)
        {
            FurnitureList furnituresInBlock = queries.FindFurnitureInBlocks(roomBlocks);
            Remove(furnituresInBlock);
        }
    }
}
