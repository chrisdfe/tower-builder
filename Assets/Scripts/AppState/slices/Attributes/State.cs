using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Attributes
{
    public class State : StateSlice
    {
        public class Input
        {
            public Residents.State.Input Residents;
            public Vehicles.State.Input Vehicles;
        }

        public Residents.State Residents;
        public Vehicles.State Vehicles;


        public State(AppState appState, Input input) : base(appState)
        {
            Residents = new Residents.State(appState, input.Residents);
            Vehicles = new Vehicles.State(appState, input.Vehicles);
        }

        public override void Setup()
        {
            base.Setup();

            Residents.Setup();
            Vehicles.Setup();
        }
    }
}