using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Behaviors.Residents
{
    public partial class ResidentBehavior
    {
        public class Goals
        {
            public Queue<Goal> queue { get; private set; } = new Queue<Goal>();

            public int Count { get => queue.Count; }

            public Goal current => queue.Count > 0 ? queue.Peek() : null;

            public Goal next => queue.Count > 1 ? queue.ToArray()[1] : null;

            public void Enqueue(Goal goal)
            {
                queue.Enqueue(goal);
            }

            public void Enqueue(Goal[] goals)
            {
                foreach (Goal goal in goals)
                {
                    queue.Enqueue(goal);
                }
            }

            public void Dequeue()
            {
                queue.Dequeue();
            }
        }

        public enum StateKey
        {
            None,
            Idle,
            Traveling,
            InteractingWithFurniture,
        }

        public Resident resident { get; private set; }

        public StateKey currentState { get; private set; } = StateKey.Idle;
        public StateKey nextState { get; private set; } = StateKey.Idle;

        public Goals goals { get; private set; } = new Goals();

        public Furniture interactionFurniture { get; private set; }

        public Route route { get; private set; }
        public RouteProgress routeProgress { get; private set; }

        AppState appState;

        public ResidentBehavior(AppState appState, Resident resident)
        {
            this.appState = appState;
            this.resident = resident;
        }

        public void Setup() { }

        public void Teardown() { }

        public void ProcessTick()
        {
            switch (currentState)
            {
                case StateKey.Idle:
                    IdleTick();
                    break;
                case StateKey.Traveling:
                    TravelingTick();
                    break;
                case StateKey.InteractingWithFurniture:
                    InteractingWithFurnitureTick();
                    break;
            }
        }

        public void BeginNextGoal()
        {
            if (goals.Count == 0) return;

            if (!goals.current.hasBegun)
            {
                goals.current.hasBegun = true;

                switch (goals.current)
                {
                    case TravelGoal travelGoal:
                        routeProgress = new RouteProgress(travelGoal.route);
                        nextState = StateKey.Traveling;
                        break;
                    case InteractingWithFurnitureGoal interactingWithFurnitureGoal:
                        interactionFurniture = interactingWithFurnitureGoal.furniture;
                        nextState = StateKey.InteractingWithFurniture;
                        break;
                }
            }
        }

        public void DefaultToIdle()
        {
            // TODO - the other way around - if current goal is complete and there's not another thing to do
            // transition to idle
            if (currentState != StateKey.Idle && goals.Count == 0)
            {
                nextState = StateKey.Idle;
            }
        }

        public void TransitionToNextState()
        {
            StateKey previousState = currentState;

            TeardownCurrentState();
            currentState = nextState;
            SetupCurrentState();

            Debug.Log($"Transitioned {resident} behavior from {previousState} to {currentState}");
        }

        public ListWrapper<ValidationError> ValidateGoal(Goal goal)
        {
            switch (goal)
            {
                case TravelGoal:
                    break;
                case InteractingWithFurnitureGoal interactingWithFurnitureGoal:
                    FurnitureBehavior furnitureBehavior = appState.Behaviors.Furnitures.queries.FindByFurniture(interactingWithFurnitureGoal.furniture);
                    furnitureBehavior.validator.Validate(appState);
                    return furnitureBehavior.validator.errors;
            }

            return new ListWrapper<ValidationError>();
        }

        // TODO - there should probably be a sepearate 'validation' pass before setup,
        //        it feels bad for validation to be happening once we've already switched
        //        to the new state
        public void SetupCurrentState()
        {
            switch (currentState)
            {
                case StateKey.Idle:
                    break;
                case StateKey.Traveling:
                    break;
                case StateKey.InteractingWithFurniture:
                    appState.Behaviors.Furnitures.StartInteraction(resident, interactionFurniture);
                    break;
            }
        }

        public void TeardownCurrentState()
        {
            switch (currentState)
            {
                case StateKey.Idle:
                    break;
                case StateKey.Traveling:
                    routeProgress = null;
                    break;
                case StateKey.InteractingWithFurniture:
                    appState.Behaviors.Furnitures.EndInteraction(resident, interactionFurniture);
                    interactionFurniture = null;
                    break;
            }
        }

        public void CompleteCurrentGoalIfItIsComplete()
        {
            switch (currentState)
            {
                case StateKey.Idle:
                    break;
                case StateKey.Traveling:
                    if (routeProgress.isAtEndOfRoute)
                    {
                        goals.current.isComplete = true;
                    }

                    break;
                case StateKey.InteractingWithFurniture:
                    // For now immediately stop interacting with furniture when there is another goal to be done
                    if (goals.Count > 1)
                    {
                        goals.current.isComplete = true;
                    }
                    break;
            }
        }

        /*
            Internals
        */
        void IdleTick()
        {
            // TODO - Look for something to do
            // TODO - wait for a bit (TickTimer) and then wander about
        }

        void InteractingWithFurnitureTick()
        {
            // TODO - check if there is anything higher priority to do
            appState.Behaviors.Furnitures.InteractWithFurniture(resident, interactionFurniture);
        }

        void TravelingTick()
        {
            routeProgress.IncrementProgress();
            appState.Entities.Residents.SetResidentPosition(resident, routeProgress.currentCell);
        }
    }
}