using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups.Rooms
{
    [Serializable]
    public class State : EntityGroupStateSlice
    {
        public struct Input
        {
            public List<Room> roomList;
        }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        /* 
            Rooms
        */
        protected void OnPreBuild(Room room)
        {

        }
    }
}
