using System.Collections.Generic;
using TowerBuilder.Stores.Rooms;

using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public class State
    {
        public List<Route> routes { get; private set; }

        public Route debugRoute;
        public List<RouteAttempt> debugRouteAttempts { get; private set; } = new List<RouteAttempt>();

        public delegate void DebugRouteEvent();
        public DebugRouteEvent onDebugRouteCalculated;
        public DebugRouteEvent onDebugRouteCleared;

        public CellCoordinates debugRouteStartCoordinates { get; private set; }
        public CellCoordinates debugRouteEndCoordinates { get; private set; }

        public delegate void DebugRouteMarkerEvent(CellCoordinates cellCoordinates);
        public DebugRouteMarkerEvent onDebugRouteStartSet;
        public DebugRouteMarkerEvent onDebugRouteEndSet;

        public State() { }

        public void SetDebugRouteStart(CellCoordinates cellCoordinates)
        {
            debugRouteStartCoordinates = cellCoordinates;

            if (onDebugRouteStartSet != null)
            {
                onDebugRouteStartSet(cellCoordinates);
            }
        }

        public void SetDebugRouteEnd(CellCoordinates cellCoordinates)
        {
            debugRouteEndCoordinates = cellCoordinates;

            if (onDebugRouteEndSet != null)
            {
                onDebugRouteEndSet(cellCoordinates);
            }
        }

        public void CalculateDebugRoute()
        {

            if (debugRouteStartCoordinates == null || debugRouteEndCoordinates == null)
            {
                Debug.Log("I need both start and end coordinates");
                return;
            }

            Debug.Log("calculating debug route");

            RouteFinder routeFinder = new RouteFinder();
            routeFinder.FindRouteBetween(debugRouteStartCoordinates, debugRouteEndCoordinates);
            Debug.Log(routeFinder.routeAttempts.Count);
            debugRouteAttempts = routeFinder.routeAttempts;

            if (onDebugRouteCalculated != null)
            {
                onDebugRouteCalculated();
            }
        }

        public void ClearDebugRoute()
        {
            debugRouteStartCoordinates = null;
            debugRouteEndCoordinates = null;

            if (onDebugRouteCleared != null)
            {
                onDebugRouteCleared();
            }
        }
    }
}
