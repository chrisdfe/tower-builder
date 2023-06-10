using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Windows;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Windows
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
            base.Setup();

            // appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            // appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            // appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            // appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
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

        /*
            Internals
        */
        void RemoveWindowsForRooms(ListWrapper<Room> roomList)
        {
            foreach (Room room in roomList.items)
            {
                // ListWrapper<Window> windowsInsideRoom =
                //     queries.GetWindowsInsideRoom(room);

                // Remove(windowsInsideRoom);
            }
        }
    }
}
