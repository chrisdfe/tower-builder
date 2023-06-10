using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Wheels
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
        }

        public override void Teardown()
        {
            base.Teardown();
        }
    }
}
