using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.Stores.Routes;

using UnityEngine;

namespace TowerBuilder.Stores.Residents
{
    public class State
    {
        public List<Resident> residents { get; private set; } = new List<Resident>();

        public delegate void ResidentsEvent(List<Resident> residents);
        public ResidentsEvent onResidentsUpdated;

        public delegate void ResidentEvent(Resident resident);
        public ResidentEvent onResidentAdded;
        public ResidentEvent onResidentDestroyed;
        public ResidentEvent onResidentPositionUpdated;

        public List<ResidentFurnitureOwnership> residentFurnitureOwnerships = new List<ResidentFurnitureOwnership>();

        Resident debugResident;

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

            List<RouteAttempt> routeAttempts = Registry.Stores.Routes.debugRouteAttempts;
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
    }
}