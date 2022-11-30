using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.TransportationItems;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class RouteFinder
    {
        public class RouteError
        {
            public string message { get; private set; } = "";

            public RouteError(string message)
            {
                this.message = message;
            }
        }

        public List<RouteAttempt> routeAttempts { get; private set; } = new List<RouteAttempt>();

        AppState appState;

        CellCoordinates startCoordinates;
        CellCoordinates endCoordinates;

        Room startRoom;
        Room endRoom;

        public List<RouteError> errors = new List<RouteError>();

        public List<RouteAttempt> sucessfulRouteAttempts
        {
            get
            {
                return routeAttempts.FindAll(routeAttempt => routeAttempt.status == RouteAttempt.Status.Complete);
            }
        }

        public RouteAttempt bestRouteAttempt
        {
            get
            {
                if (sucessfulRouteAttempts.Count == 0) return null;
                // TODO - find shortest
                return sucessfulRouteAttempts[0];
            }
        }

        public RouteFinder(AppState appState)
        {
            this.appState = appState;
        }

        public Route FindRouteBetween(CellCoordinates startCoordinates, CellCoordinates endCoordinates)
        {
            ValidateRouteMarker(startCoordinates);
            ValidateRouteMarker(endCoordinates);

            if (errors.Count > 0) return null;

            this.startCoordinates = startCoordinates;
            this.endCoordinates = endCoordinates;

            startRoom = appState.Rooms.queries.FindRoomAtCell(startCoordinates);
            endRoom = appState.Rooms.queries.FindRoomAtCell(endCoordinates);
            routeAttempts = new List<RouteAttempt>();

            RouteAttempt firstRouteAttempt = new RouteAttempt();
            firstRouteAttempt.currentCellCoordinates = startCoordinates;
            firstRouteAttempt.currentRoom = startRoom;
            firstRouteAttempt.AddVisitedRoom(startRoom);

            ContinueRouteAttempt(firstRouteAttempt);

            if (bestRouteAttempt != null)
            {
                Route newRoute = new Route(bestRouteAttempt);
                newRoute.CalculateCellSegments(appState);
                return newRoute;
            }

            return null;

            void ValidateRouteMarker(CellCoordinates cellCoordinates)
            {
                Room room = appState.Rooms.queries.FindRoomAtCell(cellCoordinates);

                // The 0th floor is a special case, since it connects to the outside world
                // if (room == null && cellCoordinates.floor != 0)
                // For now both ends have to be in a room
                if (room == null)
                {
                    errors.Add(new RouteError($"Invalid coordinates: {cellCoordinates}"));
                }
            }
        }

        void ContinueRouteAttempt(RouteAttempt currentRouteAttempt)
        {
            if (currentRouteAttempt.latestSegmentNode.room == endRoom)
            {
                CompleteRoute(currentRouteAttempt);
            }
            else
            {
                TransportationItemsList transportationItemsList = appState.TransportationItems.queries.FindTransportationItemsConnectingToRoom(currentRouteAttempt.currentRoom);

                transportationItemsList.ForEach(transportationItem =>
                {
                    UseTransportationItemIfUnvisited(currentRouteAttempt, transportationItem);
                });
            }
        }

        void UseTransportationItemIfUnvisited(RouteAttempt currentRouteAttempt, TransportationItem transportationItem)
        {
            routeAttempts.Add(currentRouteAttempt);

            RouteAttempt routeAttemptBranch = currentRouteAttempt.Clone();

            // Determine which way we're traveling through this transportation item, entrance->exit or exit->entrance
            Room entranceRoom = Registry.appState.Rooms.queries.FindRoomAtCell(transportationItem.entranceCellCoordinates);
            Room exitRoom = Registry.appState.Rooms.queries.FindRoomAtCell(transportationItem.exitCellCoordinates);

            // TODO - check if transportation item is one way
            CellCoordinates transportationStartCoordinates;
            CellCoordinates transportationEndCoordinates;
            Room nextRoom;

            if (routeAttemptBranch.currentRoom == entranceRoom)
            {
                nextRoom = exitRoom;
                transportationStartCoordinates = transportationItem.entranceCellCoordinates;
                transportationEndCoordinates = transportationItem.exitCellCoordinates;
            }
            else
            {
                nextRoom = entranceRoom;
                transportationStartCoordinates = transportationItem.exitCellCoordinates;
                transportationEndCoordinates = transportationItem.entranceCellCoordinates;
            }

            if (!routeAttemptBranch.visitedRooms.Contains(nextRoom))
            {
                // Walk from the current location to the otherRoomEntrance
                routeAttemptBranch.GoTo(
                    new RouteSegment.Node(
                        transportationStartCoordinates,
                        routeAttemptBranch.currentRoom
                    ),
                    RouteSegment.Type.WalkingAcrossRoom
                );

                // Use transportationItem and end up at the other Node
                routeAttemptBranch.GoTo(
                    new RouteSegment.Node(
                        transportationEndCoordinates,
                        nextRoom
                    ),
                    RouteSegment.Type.UsingTransportationItem
                );

                ContinueRouteAttempt(routeAttemptBranch);
            }
        }

        void CompleteRoute(RouteAttempt currentRouteAttempt)
        {
            // Add final segment from entrance to the point in room
            currentRouteAttempt.GoTo(
                new RouteSegment.Node(
                    endCoordinates,
                    endRoom
                ),
                RouteSegment.Type.WalkingAcrossRoom
            );

            // The route is now over
            routeAttempts.Add(currentRouteAttempt);
            currentRouteAttempt.status = RouteAttempt.Status.Complete;
        }
    }
}