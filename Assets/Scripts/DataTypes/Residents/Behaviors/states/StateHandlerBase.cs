using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public abstract class StateHandlerBase
    {
        protected AppState appState;

        public abstract class TransitionPayloadBase { }

        public ResidentBehavior residentBehavior { get; private set; }

        public StateHandlerBase(ResidentBehavior residentBehavior)
        {
            this.residentBehavior = residentBehavior;
        }

        public virtual void Setup(AppState appState)
        {
            this.appState = appState;
        }

        public virtual void Teardown() { }

        public abstract void ProcessTick();

        public abstract TransitionPayloadBase GetNextState();
    }
}