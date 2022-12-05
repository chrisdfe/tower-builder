using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Freights;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Freight.FreightItems
{
    using FreightListStateSlice = ListStateSlice<FreightItemList, FreightItem, State.Events>;

    public class State : FreightListStateSlice
    {
        public class Input
        {
            public FreightItemList freightItemList;
        }

        public new class Events : FreightListStateSlice.Events { }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }
        }

        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.freightItemList ?? new FreightItemList();

            queries = new Queries(appState, this);

            Setup();
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Setup() { }

        public override void Teardown() { }
    }
}