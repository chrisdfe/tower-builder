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

        public override void Setup() { }

        public override void Teardown() { }

        public new class Queries : EntityStateSlice<Floor, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }
        }
    }
}
