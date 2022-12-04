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

            public Furniture GetHomeFurnitureFor(Resident resident)
            {
                return state.furnitureHomeSlotOccupationList.GetHomeFurnitureFor(resident);
            }

            public ResidentsList GetResidentsLivingAt(Furniture furniture)
            {
                return state.furnitureHomeSlotOccupationList.GetResidentsLivingAt(furniture);
            }
        }

        public FurnitureHomeSlotOccupationList furnitureHomeSlotOccupationList { get; private set; } = new FurnitureHomeSlotOccupationList();

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);

            Setup();
        }

        public void Setup() { }

        public void Teardown() { }
    }
}