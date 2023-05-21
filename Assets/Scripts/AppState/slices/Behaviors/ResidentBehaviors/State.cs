using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.DataTypes.Validators;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Behaviors.Residents
{
    using ResidentBehaviorsListStateSlice = ListStateSlice<ResidentBehavior, State.Events>;

    public class State : ResidentBehaviorsListStateSlice
    {
        public class Input { }

        public new class Events : ResidentBehaviorsListStateSlice.Events
        {
            public ResidentBehaviorsListStateSlice.Events.ItemEvent onGoalsAdded;
            public ResidentBehaviorsListStateSlice.Events.ItemEvent onGoalBegun;
            public ResidentBehaviorsListStateSlice.Events.ItemEvent onTickProcessed;
            public ResidentBehaviorsListStateSlice.Events.ItemEvent onCurrentGoalCompleted;

            public delegate void ResidentBehaviorStateChangeEvent(ResidentBehavior residentBehavior, ResidentBehavior.StateKey previousStateKey, ResidentBehavior.StateKey newStateKey);
            public ResidentBehaviorStateChangeEvent onResidentBehaviorStateChanged;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public ResidentBehavior FindByResident(Resident resident) =>
                state.list.Find(residentBehavior => residentBehavior.resident == resident);
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);
        }

        public override void Setup()
        {
            appState.Time.events.onTick += OnTick;

            appState.Entities.Residents.events.onItemsAdded += OnResidentsAdded;
            appState.Entities.Residents.events.onItemsRemoved += OnResidentsRemoved;
            appState.Entities.Residents.events.onItemsBuilt += OnResidentsBuilt;
        }

        public override void Teardown()
        {
            appState.Time.events.onTick -= OnTick;

            appState.Entities.Residents.events.onItemsAdded -= OnResidentsAdded;
            appState.Entities.Residents.events.onItemsRemoved -= OnResidentsRemoved;
            appState.Entities.Residents.events.onItemsBuilt -= OnResidentsBuilt;
        }

        /* 
            Public Interface
         */
        public void AddBehaviorForResident(Resident resident)
        {
            ResidentBehavior residentBehavior = new ResidentBehavior(appState, resident);
            Add(residentBehavior);
        }

        public void RemoveBehaviorForResident(Resident resident)
        {
            ResidentBehavior residentBehavior = queries.FindByResident(resident);
            if (residentBehavior != null)
            {
                Remove(residentBehavior);
            }
        }

        public void AddResidentBehaviorGoals(Resident resident, ResidentBehavior.Goal[] goals)
        {
            ResidentBehavior residentBehavior = queries.FindByResident(resident);

            ListWrapper<ValidationError> validationErrors =
                goals.Aggregate(
                    new ListWrapper<ValidationError>(),
                    (acc, goal) =>
                    {
                        acc.Add(residentBehavior.ValidateGoal(goal));
                        return acc;
                    }
                );

            if (validationErrors.Count == 0)
            {
                residentBehavior.goals.Enqueue(goals);
                events.onGoalsAdded?.Invoke(residentBehavior);
            }
            else
            {
                appState.Notifications.Add(validationErrors);
            }
        }

        public void SendResidentTo(Resident resident, Furniture furniture)
        {
            Route route = FindRouteTo(resident.cellCoordinatesList.items[0], furniture.cellCoordinatesList.items[0]);

            if (route != null)
            {
                ResidentBehavior.TravelGoal travelGoal = new ResidentBehavior.TravelGoal() { route = route };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { travelGoal });
                AddFurnitureInteractionGoal(resident, furniture);
            }
        }

        public void AddFurnitureInteractionGoal(Resident resident, Furniture furniture)
        {
            ResidentBehavior.InteractingWithFurnitureGoal furnitureGoal = new ResidentBehavior.InteractingWithFurnitureGoal() { furniture = furniture };

            AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { furnitureGoal });
        }

        public void SendResidentTo(Resident resident, CellCoordinates cellCoordinates)
        {
            Route route = FindRouteTo(resident.cellCoordinatesList.items[0], cellCoordinates);

            if (route != null)
            {
                ResidentBehavior.TravelGoal travelGoal = new ResidentBehavior.TravelGoal() { route = route };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { travelGoal });
            }
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
                    appState.Notifications.Add(routeFinder.errors
);
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
                residentBehavior.ValidateGoal(residentBehavior.goals.current);

            if (validationErrors.Count == 0)
            {
                residentBehavior.ProcessTick();
                events.onTickProcessed?.Invoke(residentBehavior);
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
                events.onCurrentGoalCompleted?.Invoke(residentBehavior);

                residentBehavior.goals.Dequeue();
            }

            // Begin next goal
            if (residentBehavior.goals.current != null && !residentBehavior.goals.current.hasBegun)
            {
                residentBehavior.BeginNextGoal();

                if (events.onGoalBegun != null)
                {
                    events.onGoalBegun(residentBehavior);
                }
            }

            // Default to idle if the resident has nothing else to do 
            residentBehavior.DefaultToIdle();

            // Search for next state
            if (residentBehavior.nextState != residentBehavior.currentState)
            {
                ResidentBehavior.StateKey previousState = residentBehavior.currentState;
                residentBehavior.TransitionToNextState();

                if (events.onResidentBehaviorStateChanged != null)
                {
                    events.onResidentBehaviorStateChanged(residentBehavior, previousState, residentBehavior.nextState);
                }
            }
        }

        /* 
            Event handlers
         */
        void OnTick(TimeValue time)
        {
            list.ForEach(residentBehavior => ProcessResidentBehaviorTick(residentBehavior));
        }

        void OnResidentsAdded(ListWrapper<Resident> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                if (!resident.isInBlueprintMode)
                {
                    AddBehaviorForResident(resident);
                }
            }
        }

        void OnResidentsBuilt(ListWrapper<Resident> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                AddBehaviorForResident(resident);
            }
        }

        void OnResidentsRemoved(ListWrapper<Resident> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                RemoveBehaviorForResident(resident);
            }
        }
    }
}