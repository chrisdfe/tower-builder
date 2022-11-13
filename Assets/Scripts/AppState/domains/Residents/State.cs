using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Routes;
using TowerBuilder.State.Routes;

using UnityEngine;

namespace TowerBuilder.State.Residents
{
    public class State : StateSlice
    {
        public class Input
        {
            public ResidentsList allResidents = new ResidentsList();
        }

        public class Events
        {
            public delegate void ResidentsEvent(ResidentsList residents);
            public ResidentsEvent onResidentsAdded;
            public ResidentsEvent onResidentsRemoved;
            public ResidentsEvent onResidentsBuilt;

            public delegate void ResidentEvent(Resident resident);
            public ResidentEvent onResidentPositionUpdated;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public Resident FindResidentAtCell(CellCoordinates cellCoordinates)
            {
                return state.allResidents.FindResidentAtCell(cellCoordinates);
            }
        }

        public ResidentsList allResidents { get; private set; } = new ResidentsList();

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            if (input == null)
            {
                input = new Input();
            }

            this.allResidents = input.allResidents ?? new ResidentsList();

            events = new Events();
            queries = new Queries(this);
        }

        public void AddResident(Resident resident)
        {
            allResidents.Add(resident);

            if (events.onResidentsAdded != null)
            {
                events.onResidentsAdded(new ResidentsList(resident));
            }
        }

        public void BuildResident(Resident resident)
        {
            resident.OnBuild();

            if (events.onResidentsBuilt != null)
            {
                events.onResidentsBuilt(new ResidentsList(resident));
            }
        }

        public void RemoveResident(Resident resident)
        {
            allResidents.Remove(resident);

            Debug.Log("RemoveResident");

            if (events.onResidentsRemoved != null)
            {
                events.onResidentsRemoved(new ResidentsList(resident));
            }
        }

        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            resident.cellCoordinates = cellCoordinates;

            if (events.onResidentPositionUpdated != null)
            {
                events.onResidentPositionUpdated(resident);
            }
        }

        /*
        public void CreateDebugResidentAtCoordinates(CellCoordinates cellCoordinates)
        {
            if (debugResident != null)
            {
                allResidents.Remove(debugResident);
            }

            debugResident = new Resident();
            debugResident.coordinates = cellCoordinates;
            allResidents.Add(debugResident);

            List<RouteAttempt> routeAttempts = appState.Routes.debugRouteAttempts;
            Debug.Log("routeAttempts");
            Debug.Log(routeAttempts.Count);

            var successfulRouteAttemptQuery =
                from routeAttempt in routeAttempts
                where routeAttempt.status == RouteStatus.Complete
                select routeAttempt;
            List<RouteAttempt> successfulRouteAttempts = successfulRouteAttemptQuery.ToList();

            RouteAttempt chosenRouteAttempt = null;
            foreach (RouteAttempt successfulRouteAttempt in successfulRouteAttempts)
            {
                if (chosenRouteAttempt == null || successfulRouteAttempt.distance < chosenRouteAttempt.distance)
                {
                    chosenRouteAttempt = successfulRouteAttempt;
                }
            }

            Debug.Log("here is the chosen route attempt: ");
            Debug.Log(chosenRouteAttempt);
            if (chosenRouteAttempt == null)
            {
                Debug.Log("there is not chosen route attempt.");
                return;
            }

            Route residentRoute = new Route(chosenRouteAttempt);
            debugResident.motor.StartOnRoute(residentRoute);
            SetResidentPosition(debugResident, debugResident.coordinates);
        }

        public void AdvanceDebugResidentAlongRoute()
        {
            Debug.Log("advancing resident along route");
            debugResident.motor.ProgressAlongCurrentRoute();
            SetResidentPosition(debugResident, debugResident.coordinates);
        }

        public void ResetDebugResidentRouteProgress()
        {
            debugResident.motor.StartOnRoute(debugResident.motor.currentRoute);
            SetResidentPosition(debugResident, debugResident.coordinates);
        }
        */
    }
}