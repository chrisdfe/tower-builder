using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Furnitures
{
    using FurnitureEntityStateSlice = EntityStateSlice<FurnitureList, Furniture, State.Events>;

    [Serializable]
    public class State : FurnitureEntityStateSlice
    {
        public class Input { }

        public new class Events : FurnitureEntityStateSlice.Events { }

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
                        appState.Entities.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]) == room
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
        }

        public override void Setup()
        {
            appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;

            appState.Entities.Rooms.events.onRoomBlocksAdded += OnRoomBlocksAdded;
            appState.Entities.Rooms.events.onRoomBlocksRemoved += OnRoomBlocksRemoved;
        }

        public override void Teardown()
        {
            appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;

            appState.Entities.Rooms.events.onRoomBlocksAdded -= OnRoomBlocksAdded;
            appState.Entities.Rooms.events.onRoomBlocksRemoved -= OnRoomBlocksRemoved;
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
