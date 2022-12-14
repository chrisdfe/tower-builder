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
    using FloorEntityStateSlice = EntityStateSlice<FloorList, Floor, State.Events>;

    [Serializable]
    public class State : FloorEntityStateSlice
    {
        public class Input { }

        public new class Events : FloorEntityStateSlice.Events { }

        public StateQueries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new StateQueries(appState, this);
        }

        public override void Setup()
        {

        }

        public override void Teardown()
        {

        }

        public class StateQueries
        {
            AppState appState;
            State state;

            public StateQueries(AppState appState, State state)
            {
                this.state = state;
            }

            public Floor FindFloorAtCell(CellCoordinates cellCoordinates) =>
                state.list.FindFloorAtCell(cellCoordinates);
        }
    }
}
