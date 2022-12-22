using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Behaviors
{
    public class State : StateSlice
    {
        public class Input
        {
            public Furnitures.State.Input Furnitures;
            public Residents.State.Input Residents;
        }

        public Furnitures.State Furnitures;
        public Residents.State Residents;

        public State(AppState appState, Input input) : base(appState)
        {
            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
        }

        public override void Setup()
        {
            base.Setup();

            Furnitures.Setup();
            Residents.Setup();
        }
    }
}