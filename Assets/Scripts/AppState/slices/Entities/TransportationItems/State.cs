using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.TransportationItems
{
    public class State : EntityStateSlice
    {
        public class Input
        {
            public ListWrapper<TransportationItem> transportationItemsList;
        }

        public State(AppState appState, Input input) : base(appState) { }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}