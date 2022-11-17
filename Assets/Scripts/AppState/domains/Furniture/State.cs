using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Furnitures
{
    [Serializable]
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void FurnitureEvent(FurnitureList furnitures);
            public FurnitureEvent onFurnituresAdded;
            public FurnitureEvent onFurnituresRemoved;
            public FurnitureEvent onFurnituresBuilt;

            public delegate void FurnitureListEvent(FurnitureList furnitureList);
            public FurnitureListEvent onFurnitureListUpdated;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public FurnitureList FindFurnitureByRoom(Room room)
            {
                return new FurnitureList(
                    state.furnitureList.items.FindAll(furniture => furniture.room == room)
                );
            }

            public Furniture FindFurnitureAtCell(CellCoordinates cellCoordinates)
            {
                return state.furnitureList.FindFurnitureAtCell(cellCoordinates);
            }

            public FurnitureList FindFurnitureInBlocks(RoomBlocks roomBlocks)
            {
                List<Furniture> furnitureList = new List<Furniture>();

                foreach (RoomCells block in roomBlocks.blocks)
                {
                    foreach (CellCoordinates cellCoordinates in block.coordinatesList.items)
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

        public FurnitureList furnitureList { get; private set; } = new FurnitureList();

        public Events events;
        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();
            queries = new Queries(this);

            Setup();
        }

        public void Setup()
        {
            appState.Rooms.events.onRoomAdded += OnRoomAdded;
            appState.Rooms.events.onRoomRemoved += OnRoomRemoved;
            appState.Rooms.events.onRoomBlocksAdded += OnRoomBlocksAdded;
            appState.Rooms.events.onRoomBlocksRemoved += OnRoomBlocksRemoved;
        }

        public void Teardown()
        {
            appState.Rooms.events.onRoomAdded -= OnRoomAdded;
            appState.Rooms.events.onRoomRemoved -= OnRoomRemoved;
            appState.Rooms.events.onRoomBlocksAdded -= OnRoomBlocksAdded;
            appState.Rooms.events.onRoomBlocksRemoved -= OnRoomBlocksRemoved;
        }

        public void AddFurniture(FurnitureList furnitureList)
        {
            this.furnitureList.Add(furnitureList);

            if (events.onFurnituresAdded != null)
            {
                events.onFurnituresAdded(furnitureList);
            }
        }

        public void AddFurniture(Furniture furniture)
        {
            AddFurniture(new FurnitureList(furniture));
        }

        public void BuildFurniture(Furniture furniture)
        {
            furniture.isInBlueprintMode = false;
            furniture.OnBuild();

            if (events.onFurnituresBuilt != null)
            {
                events.onFurnituresBuilt(new FurnitureList(furniture));
            }
        }

        public void RemoveFurniture(FurnitureList furnitureList)
        {
            this.furnitureList.Remove(furnitureList);

            if (events.onFurnituresRemoved != null)
            {
                events.onFurnituresRemoved(furnitureList);
            }
        }

        public void RemoveFurniture(Furniture furniture)
        {
            RemoveFurniture(new FurnitureList(furniture));
        }

        void OnRoomAdded(Room room)
        {
            FurnitureList roomFurnitures = room.furnitureBuilder.BuildFurniture();
            AddFurniture(roomFurnitures);
        }

        void OnRoomRemoved(Room room)
        {
            FurnitureList furnituresToRemove = queries.FindFurnitureByRoom(room);
            RemoveFurniture(furnituresToRemove);
        }

        void OnRoomBlocksAdded(Room room, RoomBlocks roomBlocks)
        {
            // Recalculate furniture in room
        }

        void OnRoomBlocksRemoved(Room room, RoomBlocks roomBlocks)
        {
            FurnitureList furnituresInBlock = queries.FindFurnitureInBlocks(roomBlocks);
            RemoveFurniture(furnituresInBlock);
        }
    }
}
