using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.ResidentBehaviors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void ResidentBehaviorEvent(ResidentBehavior residentBehavior);
            public ResidentBehaviorEvent onResidentBehaviorAdded;
            public ResidentBehaviorEvent onResidentBehaviorRemoved;
            public ResidentBehaviorEvent onResidentBehaviorGoalsAdded;
            public ResidentBehaviorEvent onResidentBehaviorGoalBegun;
            public ResidentBehaviorEvent onResidentBehaviorGoalCompleted;

            public ResidentBehaviorEvent onResidentBehaviorTickProcessed;
            public ResidentBehaviorEvent onCurrentGoalCompleted;

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
                return state.residentBehaviorsList.FindByResident(resident);
            }
        }

        public ResidentBehaviorsList residentBehaviorsList { get; private set; } = new ResidentBehaviorsList();

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();
            queries = new Queries(this);

            Setup();
        }

        public void Setup()
        {
            appState.Time.events.onTick += OnTick;

            appState.Residents.events.onResidentsAdded += OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved += OnResidentsRemoved;
            appState.Residents.events.onResidentsBuilt += OnResidentsBuilt;
        }

        public void Teardown()
        {
            appState.Time.events.onTick -= OnTick;

            appState.Residents.events.onResidentsAdded -= OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved -= OnResidentsRemoved;
            appState.Residents.events.onResidentsBuilt -= OnResidentsBuilt;
        }

        /* 
            Public Interface
         */
        public void AddBehaviorForResident(Resident resident)
        {
            ResidentBehavior residentBehavior = new ResidentBehavior(appState, resident);
            residentBehavior.Setup();
            AddResidentBehavior(residentBehavior);
        }

        public void RemoveBehaviorForResident(Resident resident)
        {
            ResidentBehavior residentBehavior = residentBehaviorsList.FindByResident(resident);

            if (residentBehavior != null)
            {
                RemoveResidentBehavior(residentBehavior);
            }
        }

        public void AddResidentBehavior(ResidentBehavior residentBehavior)
        {
            residentBehaviorsList.Add(residentBehavior);

            if (events.onResidentBehaviorAdded != null)
            {
                events.onResidentBehaviorAdded(residentBehavior);
            }
        }

        public void RemoveResidentBehavior(ResidentBehavior residentBehavior)
        {
            residentBehaviorsList.Remove(residentBehavior);

            if (events.onResidentBehaviorRemoved != null)
            {
                events.onResidentBehaviorRemoved(residentBehavior);
            }
        }

        public void AddResidentBehaviorGoals(Resident resident, ResidentBehavior.GoalBase[] goals)
        {
            ResidentBehavior residentBehavior = queries.FindByResident(resident);

            foreach (ResidentBehavior.GoalBase goal in goals)
            {
                residentBehavior.goals.Enqueue(goal);
            }

            if (events.onResidentBehaviorGoalsAdded != null)
            {
                events.onResidentBehaviorGoalsAdded(residentBehavior);
            }
        }

        public void SendResidentTo(Resident resident, Furniture furniture)
        {
            Route route = FindRouteTo(resident.cellCoordinates, furniture.cellCoordinates);

            if (route != null)
            {
                ResidentBehavior.TravelGoal travelGoal = new ResidentBehavior.TravelGoal() { route = route };
                ResidentBehavior.InteractingWithFurnitureGoal furnitureGoal = new ResidentBehavior.InteractingWithFurnitureGoal() { furniture = furniture };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.GoalBase[] { travelGoal, furnitureGoal });
            }
        }

        public void SendResidentTo(Resident resident, CellCoordinates cellCoordinates)
        {
            Route route = FindRouteTo(resident.cellCoordinates, cellCoordinates);

            if (route != null)
            {
                ResidentBehavior.TravelGoal travelGoal = new ResidentBehavior.TravelGoal() { route = route };
                AddResidentBehaviorGoals(resident, new ResidentBehavior.GoalBase[] { travelGoal });
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
                appState.Notifications.AddNotification("No route found");
                return null;
            }
        }

        void ProcessResidentBehaviorTick(ResidentBehavior residentBehavior)
        {
            residentBehavior.ProcessTick();

            if (events.onResidentBehaviorTickProcessed != null)
            {
                events.onResidentBehaviorTickProcessed(residentBehavior);
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

                if (events.onResidentBehaviorGoalBegun != null)
                {
                    events.onResidentBehaviorGoalBegun(residentBehavior);
                }
            }

            // TODO - if a resident has just completed a goal and doesn't have another one to do they should automatically
            //        become idle here, instead of having to manually do that
            residentBehavior.DetermineNextState();

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
            residentBehaviorsList.ForEach(residentBehavior => ProcessResidentBehaviorTick(residentBehavior));
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