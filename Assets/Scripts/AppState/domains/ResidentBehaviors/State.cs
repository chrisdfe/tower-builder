using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using UnityEngine;

namespace TowerBuilder.State.ResidentBehaviors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {

        }

        public ResidentBehaviorsList residentBehaviorsList { get; private set; } = new ResidentBehaviorsList();

        public Events events { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            appState.Residents.events.onResidentsAdded += OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved += OnResidentsRemoved;
        }

        void OnResidentsAdded(ResidentsList residentsList) { }

        void OnResidentsRemoved(ResidentsList residentsList) { }
    }
}