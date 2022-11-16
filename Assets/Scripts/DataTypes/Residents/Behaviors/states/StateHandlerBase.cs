using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public abstract class StateHandlerBase
    {
        public abstract class TransitionPayloadBase { }

        public ResidentBehavior residentBehavior { get; private set; }

        public StateHandlerBase(ResidentBehavior residentBehavior)
        {
            this.residentBehavior = residentBehavior;
        }

        public virtual void Teardown() { }

        public abstract void ProcessTick(AppState appState);

        public abstract TransitionPayloadBase GetNextState(AppState appState);
    }
}