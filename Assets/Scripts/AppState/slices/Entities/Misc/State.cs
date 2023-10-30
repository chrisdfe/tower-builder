using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Misc
{
    [Serializable]
    public class State : EntityStateSliceBase
    {
        public class Input { }

        public State(AppState appState, Input input) : base(appState)
        {
        }
    }
}
