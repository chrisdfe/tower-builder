using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.State.Routes;

using UnityEngine;

namespace TowerBuilder.State.Residents
{
    public class State
    {
        public class Input
        {
            public List<Resident> residents = new List<Resident>();
        }

        public List<Resident> residents { get; private set; }

        public delegate void ResidentsEvent(List<Resident> residents);
        public ResidentsEvent onResidentsUpdated;

        public delegate void ResidentEvent(Resident resident);
        public ResidentEvent onResidentAdded;
        public ResidentEvent onResidentDestroyed;
        public ResidentEvent onResidentPositionUpdated;

        Resident debugResident;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            if (input == null)
            {
                input = new Input();
            }

            this.residents = input.residents;
        }

        public void AddResident(Resident resident)
        {
            residents.Add(resident);

            if (onResidentAdded != null)
            {
                onResidentAdded(resident);
            }
        }

        public void RemoveResident(Resident resident)
        {
            residents.Remove(resident);

            if (onResidentDestroyed != null)
            {
                onResidentDestroyed(resident);
            }
        }

        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            resident.coordinates = cellCoordinates;

            if (onResidentPositionUpdated != null)
            {
                onResidentPositionUpdated(resident);
            }
        }

        public void CreateDebugResidentAtCoordinates(CellCoordinates cellCoordinates)
        {
            if (debugResident != null)
            {
                RemoveResident(debugResident);
            }

            debugResident = new Resident();
            debugResident.coordinates = cellCoordinates;
            AddResident(debugResident);

            List<RouteAttempt> routeAttempts = Registry.appState.Routes.debugRouteAttempts;
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
            debugResident.StartOnRoute(residentRoute);
            SetResidentPosition(debugResident, debugResident.coordinates);
        }

        public void AdvanceDebugResidentAlongRoute()
        {
            Debug.Log("advancing resident along route");
            debugResident.ProgressAlongCurrentRoute();
            SetResidentPosition(debugResident, debugResident.coordinates);
        }

        public void ResetDebugResidentRouteProgress()
        {
            debugResident.StartOnRoute(debugResident.currentRoute);
            SetResidentPosition(debugResident, debugResident.coordinates);
        }
    }
}