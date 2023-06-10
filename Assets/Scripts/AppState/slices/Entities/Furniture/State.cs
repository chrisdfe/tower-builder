using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Furnitures
{
    [Serializable]
    public class State : EntityStateSlice
    {
        public class Input { }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            // appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            // appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            // appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;

            // appState.Entities.Rooms.events.onRoomBlocksAdded += OnRoomBlocksAdded;
            // appState.Entities.Rooms.events.onRoomBlocksRemoved += OnRoomBlocksRemoved;
        }

        public override void Teardown()
        {
            // appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            // appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            // appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;

            // appState.Entities.Rooms.events.onRoomBlocksAdded -= OnRoomBlocksAdded;
            // appState.Entities.Rooms.events.onRoomBlocksRemoved -= OnRoomBlocksRemoved;
        }

        /* 
            Event Handlers
        */
        void OnRoomsAdded(ListWrapper<Room> roomList)
        {
            // roomList.ForEach(room =>
            // {
            //     ListWrapper<Furniture> roomFurnitures = room.furnitureBuilder.BuildFurniture(room.isInBlueprintMode);
            //     Add(roomFurnitures);
            // });
        }

        void OnRoomsBuilt(ListWrapper<Room> roomList)
        {
            // roomList.ForEach(room =>
            // {
            //     ListWrapper<Furniture> roomFurnitures = queries.FindFurnitureByRoom(room);

            //     ListWrapper<Furniture> blueprintFurnitures = new ListWrapper<Furniture>(
            //         roomFurnitures.items.FindAll(roomFurniture => roomFurniture.isInBlueprintMode == true).ToList()
            //     );

            //     if (blueprintFurnitures.Count > 0)
            //     {
            //         blueprintFurnitures.ForEach(furniture =>
            //         {
            //             Build(furniture);
            //         });
            //     }
            // });
        }

        void OnRoomsRemoved(ListWrapper<Room> roomList)
        {
            // roomList.ForEach(room =>
            // {
            //     ListWrapper<Furniture> furnituresToRemove = queries.FindFurnitureByRoom(room);
            //     Remove(furnituresToRemove);
            // });
        }

        void OnRoomBlocksAdded(Room room, CellCoordinatesBlockList roomBlocks)
        {
            // Recalculate furniture in room
        }

        void OnRoomBlocksRemoved(Room room, CellCoordinatesBlockList roomBlocks)
        {
            ListWrapper<Entity> furnituresInBlock = FindFurnitureInBlocks(roomBlocks).ConvertAll<Entity>();
            Remove(furnituresInBlock);
        }

        /*
            Queries
        */
        public ListWrapper<Furniture> FindFurnituresAtCell(CellCoordinates cellCoordinates) =>
            FindEntitiesAtCell(cellCoordinates).ConvertAll<Furniture>();

        public ListWrapper<Furniture> FindFurnitureInBlocks(CellCoordinatesBlockList cellCoordinatesBlockList)
        {
            ListWrapper<Furniture> furnitureList = new ListWrapper<Furniture>();

            foreach (CellCoordinatesBlock block in cellCoordinatesBlockList.items)
            {
                foreach (CellCoordinates cellCoordinates in block.items)
                {
                    ListWrapper<Furniture> furnituresAtCell = FindFurnituresAtCell(cellCoordinates);
                    if (furnituresAtCell.Count > 0)
                    {
                        furnitureList.Add(furnituresAtCell);
                    }
                }
            }

            return furnitureList;
        }
    }
}
