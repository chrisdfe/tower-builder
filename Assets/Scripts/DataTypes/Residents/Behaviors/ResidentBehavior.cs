using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Routes;
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

        public Queue<GoalBase> goalQueue { get; private set; } = new Queue<GoalBase>();

        AppState appState;

        public ResidentBehavior(AppState appState, Resident resident)
        {
            this.appState = appState;
            this.resident = resident;
        }

        public void Setup()
        {
            currentStateHandler = new IdleStateHandler(this);
            (currentStateHandler as IdleStateHandler).Setup(appState, new IdleStateHandler.TransitionPayload());
        }

        public void Teardown()
        {
            currentStateHandler.Teardown();
        }

        public void ProcessTick(AppState appState)
        {
            currentStateHandler.ProcessTick();

            StateHandlerBase.TransitionPayloadBase nextStatePayload = currentStateHandler.GetNextState();

            if (nextStatePayload != null && nextStatePayload.key != currentStateHandler.key)
            {
                TransitionTo(nextStatePayload);
            }
        }

        public void EnqueueGoal(GoalBase goal)
        {
            goalQueue.Enqueue(goal);
        }

        public void CompleteCurrentGoal()
        {
            Debug.Log("completing current goal");
            goalQueue.Dequeue();
        }

        public GoalBase GetNextGoal()
        {
            if (goalQueue.Count == 0) return null;
            return goalQueue.Peek();
        }

        public StateHandlerBase.TransitionPayloadBase GetNextGoalTransitionPayload()
        {
            // search for goals
            GoalBase nextGoal = GetNextGoal();

            if (nextGoal != null)
            {
                switch (nextGoal)
                {
                    case TravelGoal travelGoal:
                        return new TravelingStateHandler.TransitionPayload() { route = travelGoal.route };
                    case InteractingWithFurnitureGoal furnitureGoal:
                        return new InteractingWithFurnitureStateHandler.TransitionPayload() { furniture = furnitureGoal.targetFurniture };
                }
            }

            // return to idleness
            return new IdleStateHandler.TransitionPayload();
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
                    (currentStateHandler as IdleStateHandler).Setup(appState, idleStatePayload);
                    break;
                case TravelingStateHandler.TransitionPayload travelingStatePayload:
                    currentState = StateType.Traveling;
                    currentStateHandler = new TravelingStateHandler(this);
                    (currentStateHandler as TravelingStateHandler).Setup(appState, travelingStatePayload);
                    break;
                case InteractingWithFurnitureStateHandler.TransitionPayload interactingWithFurnitureStatePayload:
                    currentState = StateType.InteractingWithFurniture;
                    currentStateHandler = new InteractingWithFurnitureStateHandler(this);
                    (currentStateHandler as InteractingWithFurnitureStateHandler).Setup(appState, interactingWithFurnitureStatePayload);
                    break;
            }

            Debug.Log($"Transitioned {resident} behavior from {previousState} to {currentState}");
        }
    }
}