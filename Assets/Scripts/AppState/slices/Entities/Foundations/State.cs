using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Foundations
{
    [Serializable]
    public class State : EntityStateSlice<Foundation, State.Events>
    {
        public class Input { }

        public new class Events : EntityStateSlice<Foundation, State.Events>.Events { }

        public new Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public new class Queries : EntityStateSlice<Foundation, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }
        }
    }
}
