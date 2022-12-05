using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Attributes;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.ResidentBehaviors
{
    using ResidentBehaviorsListStateSlice = ListStateSlice<ResidentBehaviorsList, ResidentBehavior, State.Events>;

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

            public ResidentBehavior FindByResident(Resident resident)
            {
                return state.list.FindByResident(resident);
            }
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);

            Setup();
        }

        public override void Setup()
        {
            appState.Time.events.onTick += OnTick;

            appState.Residents.events.onItemsAdded += OnResidentsAdded;
            appState.Residents.events.onItemsRemoved += OnResidentsRemoved;
            appState.Residents.events.onItemsBuilt += OnResidentsBuilt;
        }

        public override void Teardown()
        {
            appState.Time.events.onTick -= OnTick;

            appState.Residents.events.onItemsAdded -= OnResidentsAdded;
            appState.Residents.events.onItemsRemoved -= OnResidentsRemoved;
            appState.Residents.events.onItemsBuilt -= OnResidentsBuilt;
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
            ResidentBehavior residentBehavior = list.FindByResident(resident);
            if (residentBehavior != null)
            {
                Remove(residentBehavior);
            }
        }


        public void AddResidentBehaviorGoals(Resident resident, ResidentBehavior.Goal[] goals)
        {
            ResidentBehavior residentBehavior = queries.FindByResident(resident);

            foreach (ResidentBehavior.Goal goal in goals)
            {
                residentBehavior.goals.Enqueue(goal);
            }

            events.onGoalsAdded?.Invoke(residentBehavior);
        }

        public void SendResidentTo(Resident resident, Furniture furniture)
        {
            Route route = FindRouteTo(resident.cellCoordinates, furniture.cellCoordinates);

            if (route != null)
            {
                ResidentBehavior.TravelGoal travelGoal = new ResidentBehavior.TravelGoal() { route = route };
                ResidentBehavior.InteractingWithFurnitureGoal furnitureGoal = new ResidentBehavior.InteractingWithFurnitureGoal() { furniture = furniture };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.Goal[] { travelGoal, furnitureGoal });
            }
        }

        public void SendResidentTo(Resident resident, CellCoordinates cellCoordinates)
        {
            Route route = FindRouteTo(resident.cellCoordinates, cellCoordinates);

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
            Route route = new RouteFinder(appState).FindRouteBetween(fromCellCoordinates, toCellCoordinates);

            if (route != null)
            {
                return route;
            }
            else
            {
                appState.Notifications.Add(new Notification("No route found"));
                return null;
            }
        }

        void ProcessResidentBehaviorTick(ResidentBehavior residentBehavior)
        {
            // 
            residentBehavior.ProcessTick();

            if (events.onTickProcessed != null)
            {
                events.onTickProcessed(residentBehavior);
            }

            // Remove current goal if it has been marked as complete
            residentBehavior.CompleteCurrentGoalIfItIsComplete();

            if (residentBehavior.goals.current != null && residentBehavior.goals.current.isComplete)
            {
                if (events.onCurrentGoalCompleted != null)
                {
                    events.onCurrentGoalCompleted(residentBehavior);
                }

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

        void OnResidentsAdded(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                if (!resident.isInBlueprintMode)
                {
                    AddBehaviorForResident(resident);
                }
            }
        }

        void OnResidentsBuilt(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                AddBehaviorForResident(resident);
            }
        }

        void OnResidentsRemoved(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                RemoveBehaviorForResident(resident);
            }
        }
    }
}