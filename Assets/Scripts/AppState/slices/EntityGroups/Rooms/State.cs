using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
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
