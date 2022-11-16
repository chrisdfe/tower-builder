using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class ResidentBehavior
    {
        public enum StateType
        {
            Idle,
            Traveling,
            InteractingWithFurniture
        }

        public Resident resident { get; private set; }

        public StateType currentState { get; private set; } = StateType.Idle;

        public StateHandlerBase currentStateHandler { get; private set; }

        public ResidentBehavior(Resident resident)
        {
            this.resident = resident;
            currentStateHandler = GetStateHandlerForStateType(currentState);
        }

        public void Setup()
        {
            Debug.Log("Resident behavior created");
        }

        public void Teardown()
        {
            currentStateHandler.Teardown();
        }

        public void ProcessTick(AppState appState)
        {
            currentStateHandler.ProcessTick(appState);

            StateHandlerBase.TransitionPayloadBase nextStatePayload = currentStateHandler.GetNextState(appState);

            if (nextStatePayload != null)
            {
                TransitionTo(nextState);
            }
        }

        void TransitionTo(TransitionPayloadBase nextState)
        {
            if (nextState == currentState) return;
            Debug.Log($"Transitioning {resident} behavior from {currentState} to {nextState}");
            currentStateHandler.Teardown();

            currentState = nextState;
            currentStateHandler = GetStateHandlerForStateType(currentState);
            currentStateHandler.Setup();
        }

        StateHandlerBase GetStateHandlerForStateType(StateType stateType)
        {
            switch (currentState)
            {
                case StateType.Idle:
                    return new IdleStateHandler(this);
                case StateType.Traveling:
                    return new TravelingStateHandler(this);
                case StateType.InteractingWithFurniture:
                    return new InteractingWithFurnitureStateHandler(this);
            }

            return new IdleStateHandler(this);
        }
    }
}