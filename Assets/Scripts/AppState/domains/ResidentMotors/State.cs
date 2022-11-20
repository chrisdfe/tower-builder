using System;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Motors;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.ResidentMotors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void ResidentMotorEvent(ResidentMotor residentMotor);
            public ResidentMotorEvent onResidentMotorAdded;
            public ResidentMotorEvent onResidentMotorRemoved;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;

            }

            public ResidentMotor FindByResident(Resident resident)
            {
                return state.residentMotorsList.FindByResident(resident);
            }
        }

        public ResidentMotorsList residentMotorsList { get; private set; } = new ResidentMotorsList();

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
        public void AddMotorForResident(Resident resident)
        {
            ResidentMotor residentMotor = new ResidentMotor(resident);
            residentMotor.Setup();
            AddResidentMotor(residentMotor);
        }

        public void RemoveMotorForResident(Resident resident)
        {
            ResidentMotor residentMotor = residentMotorsList.FindByResident(resident);

            if (residentMotor != null)
            {
                RemoveResidentMotor(residentMotor);
            }
        }

        public void AddResidentMotor(ResidentMotor residentMotor)
        {
            residentMotorsList.Add(residentMotor);

            Debug.Log("resident motor added");

            if (events.onResidentMotorAdded != null)
            {
                events.onResidentMotorAdded(residentMotor);
            }
        }

        public void RemoveResidentMotor(ResidentMotor residentMotor)
        {
            residentMotorsList.Remove(residentMotor);

            Debug.Log("resident motor removed");

            if (events.onResidentMotorRemoved != null)
            {
                events.onResidentMotorRemoved(residentMotor);
            }
        }

        public Route FindRouteTo(Resident resident, Furniture furniture)
        {
            ResidentMotor residentMotor = residentMotorsList.FindByResident(resident);

            // for now if a resident is not in a room then don't attempt to find a route
            Room currentResidentRoom = appState.Rooms.queries.FindRoomAtCell(resident.cellCoordinates);
            if (currentResidentRoom == null)
            {
                throw new NotSupportedException("Residents must be in a room to find a route");
            }

            Debug.Log("time to find a route");

            return null;
        }

        /* 
            Event handlers
         */
        void OnTick(TimeValue time)
        {
        }

        void OnResidentsAdded(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                if (!resident.isInBlueprintMode)
                {
                    AddMotorForResident(resident);
                }
            }
        }

        void OnResidentsBuilt(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                AddMotorForResident(resident);
            }
        }

        void OnResidentsRemoved(ResidentsList residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                RemoveMotorForResident(resident);
            }
        }
    }
}