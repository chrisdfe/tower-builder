using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Behaviors.Residents
{
    public class ResidentBehavior
    {
        public abstract class Goal
        {
            public virtual string title { get; } = "Goal";
            public bool isComplete = false;
            public bool hasBegun = false;
        }

        public class TravelGoal : Goal
        {
            public override string title { get => "Walk"; }
            public Route route;
        }

        public class InteractingWithFurnitureGoal : Goal
        {
            public override string title { get => $"Use {furniture}"; }
            public Furniture furniture;
        }

        public class Goals
        {
            public Queue<Goal> queue { get; private set; } = new Queue<Goal>();

            public int Count { get => queue.Count; }

            public Goal current
            {
                get
                {
                    if (queue.Count == 0) return null;
                    return queue.Peek();
                }
            }

            public void Enqueue(Goal goal)
            {
                queue.Enqueue(goal);
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

        public void SetupCurrentState()
        {
            switch (currentState)
            {
                case StateKey.Idle:
                    break;
                case StateKey.Traveling:
                    break;
                case StateKey.InteractingWithFurniture:
                    appState.FurnitureBehaviors.StartInteraction(resident, interactionFurniture);
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
                    appState.FurnitureBehaviors.EndInteraction(resident, interactionFurniture);
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
                        // nextState = StateKey.Idle;
                    }

                    break;
                case StateKey.InteractingWithFurniture:
                    // For now immediately stop interacting with furniture when there is another goal to be done
                    if (goals.Count > 1)
                    {
                        goals.current.isComplete = true;
                        // nextState = StateKey.Idle;
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
            appState.FurnitureBehaviors.InteractwithFurniture(resident, interactionFurniture);
        }

        void TravelingTick()
        {
            routeProgress.IncrementProgress();
            appState.Entities.Residents.SetResidentPosition(resident, routeProgress.currentCell);
        }
    }
}