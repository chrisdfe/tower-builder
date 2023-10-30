using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Floors
{
    [Serializable]
    public class State : EntityStateSliceBase
    {
        public class Input { }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            base.Setup();

            // appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            // appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            // appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            // appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            // appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            // appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
        }

        /*
            Internals
        */
        void AddFloorsForRooms(ListWrapper<Room> roomList)
        {
            // FloorDefinition defaultFloorDefinition =
            //     Registry.Definitions.Entities.Floors.defaultDefinition as FloorDefinition;

            // foreach (Room room in roomList.items)
            // {
            //     CellCoordinatesList bottomRow = room.cellCoordinatesList.bottomRow;

            //     List<Floor> floorsToAdd = bottomRow.items.Select((cellCoordinates) =>
            //     {
            //         Floor newFloor = new Floor(defaultFloorDefinition);
            //         newFloor.PositionAtCoordinates(cellCoordinates);
            //         newFloor.isInBlueprintMode = true;
            //         newFloor.room = room;
            //         return newFloor;
            //     }).ToList();

            //     Add(new ListWrapper<Floor>(floorsToAdd));
            // }
        }

        void RemoveFloorsForRooms(ListWrapper<Room> roomList)
        {
            // foreach (Room room in roomList.items)
            // {
            //     ListWrapper<Floor> floorsInsideRoom =
            //         queries.GetFloorsInsideRoom(room);

            //     Remove(floorsInsideRoom);
            // }
        }

        /*
            Event Handlers
        */
        void OnRoomsAdded(ListWrapper<Room> roomsList)
        {
            // AddFloorsForRooms(roomsList);
        }

        void OnRoomsBuilt(ListWrapper<Room> roomsList)
        {
            // RemoveFloorsForRooms(roomsList);
            // AddFloorsForRooms(roomsList);

            // foreach (Room room in roomsList.items)
            // {
            //     ListWrapper<Floor> floorsInsideRoom = queries.GetFloorsInsideRoom(room);

            //     foreach (Floor floor in floorsInsideRoom.items)
            //     {
            //         Build(floor);
            //     }
            // }
        }

        void OnRoomsRemoved(ListWrapper<Room> roomsList)
        {
            // RemoveFloorsForRooms(roomsList);
        }
    }
}
