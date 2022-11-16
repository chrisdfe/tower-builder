using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Time;
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

            currentStateHandler = new IdleStateHandler(this);
            (currentStateHandler as IdleStateHandler).Setup(new IdleStateHandler.TransitionPayload());
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
                TransitionTo(nextStatePayload);
            }
        }

        void TransitionTo(StateHandlerBase.TransitionPayloadBase nextStatePayload)
        {
            StateType previousState = currentState;
            currentStateHandler.Teardown();

            switch (nextStatePayload)
            {
                case IdleStateHandler.TransitionPayload idleStatePayload:
                    currentState = StateType.Idle;
                    currentStateHandler = new IdleStateHandler(this);
                    (currentStateHandler as IdleStateHandler).Setup(idleStatePayload);
                    break;
                case TravelingStateHandler.TransitionPayload travelingStatePayload:
                    currentState = StateType.Traveling;
                    currentStateHandler = new TravelingStateHandler(this);
                    (currentStateHandler as TravelingStateHandler).Setup(travelingStatePayload);
                    break;
                case InteractingWithFurnitureStateHandler.TransitionPayload interactingWithFurnitureStatePayload:
                    currentState = StateType.InteractingWithFurniture;
                    currentStateHandler = new InteractingWithFurnitureStateHandler(this);
                    (currentStateHandler as InteractingWithFurnitureStateHandler).Setup(interactingWithFurnitureStatePayload);
                    break;
            }

            Debug.Log($"Transitioned {resident} behavior from {previousState} to {currentState}");
        }
    }
}