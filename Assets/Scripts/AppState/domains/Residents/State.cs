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
    public class State
    {
        public class Input
        {
            public List<Resident> allResidents = new List<Resident>();
        }

        public ResourceList<Resident> allResidents { get; private set; } = new ResourceList<Resident>();

        public delegate void ResidentEvent(Resident resident);
        public ResidentEvent onResidentPositionUpdated;

        Resident debugResident;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            if (input == null)
            {
                input = new Input();
            }

            this.allResidents.Set(input.allResidents);
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
                allResidents.Remove(debugResident);
            }

            debugResident = new Resident();
            debugResident.coordinates = cellCoordinates;
            allResidents.Add(debugResident);

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