using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Residents
{
    public class State : EntityStateSlice
    {
        public class Input
        {
            public ListWrapper<Resident> residentsList = new ListWrapper<Resident>();
        }

        /*
            Events
        */
        public ItemEvent<Resident> onItemPositionUpdated;

        public ItemEvent<ResidentBehavior> onGoalsAdded;
        public ItemEvent<ResidentBehavior> onGoalBegun;
        public ItemEvent<ResidentBehavior> onCurrentGoalCompleted;
        public ItemEvent<ResidentBehavior> onTickProcessed;

        public delegate void ResidentBehaviorStateChangeEvent(ResidentBehavior residentBehavior, ResidentBehavior.StateKey previousStateKey, ResidentBehavior.StateKey newStateKey);
        public ResidentBehaviorStateChangeEvent onResidentBehaviorStateChanged;

        public State(AppState appState, Input input) : base(appState) { }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Setup()
        {
            base.Setup();
            appState.Time.onTick += OnTick;
        }

        /*
            Public Interface
        */
        public void AddResidentBehaviorGoals(Resident resident, ResidentBehavior.Goal[] goals)
        {
            ListWrapper<ValidationError> validationErrors =
                goals.Aggregate(
                    new ListWrapper<ValidationError>(),
                    (acc, goal) =>
                    {
                        acc.Add(resident.behavior.ValidateGoal(goal, appState));
                        return acc;
                    }
                );

            if (validationErrors.Count == 0)
            {
                resident.behavior.goals.Enqueue(goals);
                onGoalsAdded?.Invoke(resident.behavior);
            }
            else
            {
                appState.Notifications.Add(validationErrors);
            }
        }

        public void SendResidentTo(Resident resident, CellCoordinates targetCellCoordinates)
        {
            Route route = FindRouteTo(
                appState.EntityGroups.GetAbsoluteCellCoordinatesList(resident).items[0],
                targetCellCoordinates
            );

            if (route != null)
            {
                TravelGoal travelGoal = new TravelGoal() { route = route };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { travelGoal });
            }
        }

        public void SendResidentTo(Resident resident, Furniture furniture)
        {
            Route route = FindRouteTo(
                appState.EntityGroups.GetAbsoluteCellCoordinatesList(resident).items[0],
                appState.EntityGroups.GetAbsoluteCellCoordinatesList(furniture).items[0]
            );

            if (route != null)
            {
                TravelGoal travelGoal = new TravelGoal() { route = route };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { travelGoal });
                AddFurnitureInteractionGoal(resident, furniture);
            }
        }

        public void AddFurnitureInteractionGoal(Resident resident, Furniture furniture)
        {
            InteractingWithFurnitureGoal furnitureGoal = new InteractingWithFurnitureGoal() { furniture = furniture };

            AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { furnitureGoal });
        }

        /* 
            Event handlers
         */
        void OnTick(TimeValue time)
        {
            list.ForEach(resident => ProcessResidentBehaviorTick((resident as Resident).behavior));
        }

        /*
            Internals
        */
        Route FindRouteTo(CellCoordinates fromCellCoordinates, CellCoordinates toCellCoordinates)
        {
            RouteFinder routeFinder = new RouteFinder(appState);
            Route route = routeFinder.FindRouteBetween(fromCellCoordinates, toCellCoordinates);

            if (route != null)
            {
                return route;
            }
            else
            {
                if (routeFinder.errors.Count > 0)
                {
                    appState.Notifications.Add(routeFinder.errors);
                }
                else
                {
                    appState.Notifications.Add(new Notification("No route found"));
                }

                return null;
            }
        }

        void ProcessResidentBehaviorTick(ResidentBehavior residentBehavior)
        {
            // 
            ListWrapper<ValidationError> validationErrors =
                residentBehavior.ValidateGoal(residentBehavior.goals.current, appState);

            if (validationErrors.Count == 0)
            {
                residentBehavior.ProcessTick();
                onTickProcessed?.Invoke(residentBehavior);
            }
            else
            {
                // handle invalid furniture interaction
                // for now just complete the current goal and move on to the next thing
                residentBehavior.goals.current.isComplete = true;
            }

            // Remove current goal if it has been marked as complete
            residentBehavior.CompleteCurrentGoalIfItIsComplete();

            if (residentBehavior.goals.current != null && residentBehavior.goals.current.isComplete)
            {
                onCurrentGoalCompleted?.Invoke(residentBehavior);

                residentBehavior.goals.Dequeue();
            }

            // Begin next goal
            if (residentBehavior.goals.current != null && !residentBehavior.goals.current.hasBegun)
            {
                residentBehavior.BeginNextGoal();

                if (onGoalBegun != null)
                {
                    onGoalBegun(residentBehavior);
                }
            }

            // Default to idle if the resident has nothing else to do 
            residentBehavior.DefaultToIdle();

            // Search for next state
            if (residentBehavior.nextState != residentBehavior.currentState)
            {
                ResidentBehavior.StateKey previousState = residentBehavior.currentState;
                residentBehavior.TransitionToNextState(appState);

                if (onResidentBehaviorStateChanged != null)
                {
                    onResidentBehaviorStateChanged(residentBehavior, previousState, residentBehavior.nextState);
                }
            }
        }
    }
}