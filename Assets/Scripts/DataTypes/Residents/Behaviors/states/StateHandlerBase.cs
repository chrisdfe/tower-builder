using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public abstract class StateHandlerBase
    {
        public ResidentBehavior residentBehavior { get; private set; }

        public abstract class TransitionPayloadBase
        {
            public virtual ResidentBehavior.StateType stateType { get; }
        }

        public StateHandlerBase(ResidentBehavior residentBehavior)
        {
            this.residentBehavior = residentBehavior;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public abstract void ProcessTick(AppState appState);

        public abstract TransitionPayloadBase GetNextState(AppState appState);
    }
}