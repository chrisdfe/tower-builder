using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Residents;
using TowerBuilder.Stores.Time;

namespace TowerBuilder.Stores.ResidentBehaviors
{
    public class State
    {
        public State()
        {
            Registry.Stores.Time.onTick += OnTick;
            Registry.Stores.Residents.onResidentAdded += OnResidentAdded;
        }

        void OnTick(TimeValue timeValue)
        {

        }

        void OnResidentAdded(Resident resdient)
        {

        }
    }
}
