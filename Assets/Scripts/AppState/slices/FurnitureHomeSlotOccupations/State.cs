using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState.FurnitureHomeSlotOccupations
{
    using FurnitureHomeSlotOccupiationStateSlice = ListStateSlice<FurnitureHomeSlotOccupationList, FurnitureHomeSlotOccupation, State.Events>;

    public class State : FurnitureHomeSlotOccupiationStateSlice
    {
        public class Input { }

        public new class Events : FurnitureHomeSlotOccupiationStateSlice.Events { }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public Furniture GetHomeFurnitureFor(Resident resident) =>
                state.list.GetHomeFurnitureFor(resident);

            public ResidentsList GetResidentsLivingAt(Furniture furniture) =>
                state.list.GetResidentsLivingAt(furniture);
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);

            Setup();
        }

        public override void Setup() { }

        public override void Teardown() { }
    }
}