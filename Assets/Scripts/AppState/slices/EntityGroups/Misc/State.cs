using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.EntityGroups.Buildings;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups.Misc
{
    public class State : EntityGroupStateSlice
    {
        public class Input
        {
            public ListWrapper<EntityGroup> entityGroups;
        }

        public State(AppState appState, Input input) : base(appState)
        {
        }
    }
}