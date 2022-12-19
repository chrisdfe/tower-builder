using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Floors
{
    [Serializable]
    public class State : EntityStateSlice<Floor, State.Events>
    {
        public class Input { }

        public new class Events : EntityStateSlice<Floor, State.Events>.Events { }

        public new Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public override void Setup()
        {
            base.Setup();
            appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
        }

        /* 
            Event Handlers
        */
        public void OnRoomsAdded(ListWrapper<Room> roomsList)
        {
            FloorDefinition defaultFloorDefinition =
                Registry.Definitions.Entities.Floors.defaultDefinition as FloorDefinition;

            foreach (Room room in roomsList.items)
            {
                CellCoordinatesList bottomRow = room.cellCoordinatesList.bottomRow;

                List<Floor> floorsToAdd = bottomRow.items.Select((cellCoordinates) =>
                {
                    Floor newFloor = new Floor(defaultFloorDefinition);
                    newFloor.PositionAtCoordinates(cellCoordinates);
                    newFloor.isInBlueprintMode = true;
                    return newFloor;
                }).ToList();

                Add(new ListWrapper<Floor>(floorsToAdd));
            }
        }

        public void OnRoomsBuilt(ListWrapper<Room> roomsList)
        {
            foreach (Room room in roomsList.items)
            {
                ListWrapper<Floor> floorsInsideRoom = queries.GetFloorsInsideRoom(room);

                foreach (Floor floor in floorsInsideRoom.items)
                {
                    Build(floor);
                }
            }
        }

        public void OnRoomsRemoved(ListWrapper<Room> roomsList)
        {
            foreach (Room room in roomsList.items)
            {
                ListWrapper<Floor> floorsInsideRoom =
                    queries.GetFloorsInsideRoom(room).FindAll((floor) =>
                        floor.isInBlueprintMode == room.isInBlueprintMode
                    );
                Remove(floorsInsideRoom);
            }
        }

        public new class Queries : EntityStateSlice<Floor, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }

            public ListWrapper<Floor> GetFloorsInsideRoom(Room room) =>
                state.list.FindAll((floor) =>
                    floor.cellCoordinatesList.OverlapsWith(room.cellCoordinatesList)
                );

            public ListWrapper<Floor> GetFloorsOnFloor(int floorNumber) =>
                state.list.FindAll((floor) => floor.cellCoordinatesList.floorValues.Contains(floorNumber));

            public ListWrapper<Floor> GetFloorsInsideRoomOnFloor(Room room, int floorNumber) =>
                GetFloorsInsideRoom(room).FindAll((floor) =>
                    floor.cellCoordinatesList.floorValues.Contains(floorNumber)
                );
        }
    }
}
