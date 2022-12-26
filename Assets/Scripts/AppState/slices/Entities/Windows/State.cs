using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Windows;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Windows
{
    [Serializable]
    public class State : EntityStateSlice<Window, State.Events>
    {
        public class Input { }

        public new class Events : EntityStateSlice<Window, State.Events>.Events { }

        public new Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
        }

        /* 
            Event Handlers
        */
        public void OnRoomsAdded(ListWrapper<Room> roomsList)
        {
        }

        public void OnRoomsRemoved(ListWrapper<Room> roomsList)
        {
            RemoveWindowsForRooms(roomsList);
        }

        void RemoveWindowsForRooms(ListWrapper<Room> roomList)
        {
            foreach (Room room in roomList.items)
            {
                ListWrapper<Window> windowsInsideRoom =
                    queries.GetWindowsInsideRoom(room);

                Remove(windowsInsideRoom);
            }
        }

        public new class Queries : EntityStateSlice<Window, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }

            public ListWrapper<Window> GetWindowsInsideRoom(Room room) =>
                state.list.FindAll((window) => window.room == room);
        }
    }
}
