using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Residents.Motors;
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
            public ResidentBehaviorEvent onResidentBehaviorGoalCompleted;

            public delegate void ResidentBehaviorTravelingStateHandlerEvent(ResidentBehavior residentBehavior, TravelingStateHandler travelingStateHandler);
            public ResidentBehaviorTravelingStateHandlerEvent onResidentBehaviorTraveled;

            public delegate void ResidentBehaviorStateChangeEvent(ResidentBehavior residentBehavior, StateHandlerBase.StateKey previousStateKey, StateHandlerBase.StateKey newStateKey);
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

        public void AddResidentBehaviorGoals(Resident resident, GoalBase[] goals)
        {
            ResidentBehavior residentBehavior = queries.FindByResident(resident);

            foreach (GoalBase goal in goals)
            {
                residentBehavior.EnqueueGoal(goal);
            }

            if (events.onResidentBehaviorGoalsAdded != null)
            {
                events.onResidentBehaviorGoalsAdded(residentBehavior);
            }
        }

        public void SendResidentTo(Resident resident, Furniture furniture)
        {
            SendResidentTo(resident, furniture.cellCoordinates);
        }

        public void SendResidentTo(Resident resident, CellCoordinates cellCoordinates)
        {
            Route route = new RouteFinder(appState).FindRouteBetween(resident.cellCoordinates, cellCoordinates);

            if (route != null)
            {
                TravelGoal travelGoal = new TravelGoal() { route = route };

                AddResidentBehaviorGoals(resident, new GoalBase[] { travelGoal });
            }
            else
            {
                appState.Notifications.AddNotification("No route found");
            }
        }

        /* 
            Internals
        */
        void ProcessResidentBehaviorTick(ResidentBehavior residentBehavior)
        {
            residentBehavior.ProcessTick(appState);

            Debug.Log("residentBehavior.currentStateHandler.key");
            Debug.Log(residentBehavior.currentStateHandler.key);
            switch (residentBehavior.currentStateHandler.key)
            {
                case StateHandlerBase.StateKey.Idle:
                    break;
                case StateHandlerBase.StateKey.InteractingWithFurniture:
                    break;
                case StateHandlerBase.StateKey.Traveling:
                    if (events.onResidentBehaviorTraveled != null)
                    {
                        TravelingStateHandler handler = (residentBehavior.currentStateHandler as TravelingStateHandler);
                        events.onResidentBehaviorTraveled(residentBehavior, handler);
                    }
                    break;
            }

            // TODO - on resident behavior tick instead?

            // TODO - check if goal has completed?

            StateHandlerBase.TransitionPayloadBase nextStatePayload = residentBehavior.currentStateHandler.GetNextState();

            if (nextStatePayload != null && nextStatePayload.key != residentBehavior.currentStateHandler.key)
            {
                StateHandlerBase.StateKey previousStateKey = residentBehavior.currentStateHandler.key;
                residentBehavior.TransitionTo(nextStatePayload);
                StateHandlerBase.StateKey currentStateKey = residentBehavior.currentStateHandler.key;

                if (events.onResidentBehaviorStateChanged != null)
                {
                    events.onResidentBehaviorStateChanged(residentBehavior, previousStateKey, currentStateKey);
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