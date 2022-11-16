using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Residents.Motors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.State.ResidentMotors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void ResidentBehaviorEvent(ResidentBehavior residentBehavior);
            public ResidentBehaviorEvent onResidentBehaviorAdded;
            public ResidentBehaviorEvent onResidentBehaviorRemoved;
        }

        public ResidentMotorsList ResidentMotorsList { get; private set; } = new ResidentMotorsList();

        public Events events { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();

            Setup();
        }

        public void Setup()
        {
            appState.Time.onTick += OnTick;

            appState.Residents.events.onResidentsAdded += OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved += OnResidentsRemoved;
            appState.Residents.events.onResidentsBuilt += OnResidentsBuilt;
        }

        public void Teardown()
        {
            appState.Time.onTick -= OnTick;

            appState.Residents.events.onResidentsAdded -= OnResidentsAdded;
            appState.Residents.events.onResidentsRemoved -= OnResidentsRemoved;
            appState.Residents.events.onResidentsBuilt -= OnResidentsBuilt;
        }

        /* 
            Event handlers
         */
        void OnTick(TimeValue time)
        {
            foreach (ResidentBehavior residentBehavior in ResidentMotorsList.items)
            {
                residentBehavior.ProcessTick(appState);
            }
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

        /* 
            Public API
         */
        public void AddBehaviorForResident(Resident resident)
        {
            ResidentBehavior residentBehavior = new ResidentBehavior(resident);
            residentBehavior.Setup();
            AddResidentBehavior(residentBehavior);
        }

        public void RemoveBehaviorForResident(Resident resident)
        {
            ResidentBehavior residentBehavior = ResidentMotorsList.FindByResident(resident);
            Debug.Log("findbyresidnet: ");
            Debug.Log(residentBehavior);
            if (residentBehavior != null)
            {
                RemoveResidentBehavior(residentBehavior);
            }

            Debug.Log(ResidentMotorsList.Count);
        }

        public void AddResidentBehavior(ResidentBehavior residentBehavior)
        {
            ResidentMotorsList.Add(residentBehavior);

            if (events.onResidentBehaviorAdded != null)
            {
                events.onResidentBehaviorAdded(residentBehavior);
            }
        }

        public void RemoveResidentBehavior(ResidentBehavior residentBehavior)
        {
            ResidentMotorsList.Remove(residentBehavior);

            if (events.onResidentBehaviorRemoved != null)
            {
                events.onResidentBehaviorRemoved(residentBehavior);
            }
        }
    }
}