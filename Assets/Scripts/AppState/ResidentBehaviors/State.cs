using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Residents;
using TowerBuilder.State.Time;

namespace TowerBuilder.State.ResidentBehaviors
{
    public class State
    {
        public State()
        {
        }

        public void Setup()
        {
            Registry.appState.Time.onTick += OnTick;
            Registry.appState.Residents.onResidentAdded += OnResidentAdded;
        }

        void OnTick(TimeValue timeValue)
        {

        }

        void OnResidentAdded(Resident resdient)
        {

        }
    }
}
