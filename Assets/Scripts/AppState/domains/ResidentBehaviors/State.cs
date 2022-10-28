using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;

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
