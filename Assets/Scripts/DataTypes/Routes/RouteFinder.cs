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

        RouteAttempt currentRouteAttempt;

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

            BeginFirstRouteAttempt();

            Debug.Log("done.");
            Debug.Log("routeAttempts: ");
            Debug.Log(routeAttempts.Count);
            Debug.Log("sucessfulRouteAttempts: ");
            Debug.Log(sucessfulRouteAttempts.Count);
            Debug.Log("bestRouteAttempt");
            Debug.Log(bestRouteAttempt);

            if (bestRouteAttempt != null)
            {
                return new Route(bestRouteAttempt);
            }

            return null;

            void BeginFirstRouteAttempt()
            {
                routeAttempts = new List<RouteAttempt>();

                RouteAttempt firstRouteAttempt = new RouteAttempt(new List<Room>(), new List<RouteSegment>());
                firstRouteAttempt.currentCellCoordinates = startCoordinates;
                firstRouteAttempt.currentRoom = startRoom;
                firstRouteAttempt.AddVisitedRoom(startRoom);

                ContinueRouteAttempt(firstRouteAttempt);
            }

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
            this.currentRouteAttempt = currentRouteAttempt;

            if (currentRouteAttempt.latestSegmentNode.room == endRoom)
            {
                CompleteRoute();
            }
            else
            {
                TransportationItemsList transportationItemsList = appState.TransportationItems.queries.FindTransportationItemsConnectingToRoom(currentRouteAttempt.currentRoom);

                Debug.Log($"Found {transportationItemsList.Count} transportation items to use");

                transportationItemsList.ForEach(transportationItem =>
                {
                    UseTransportationItemIfUnvisited(transportationItem);
                });
            }
        }

        void UseTransportationItemIfUnvisited(TransportationItem transportationItem)
        {
            routeAttempts.Add(currentRouteAttempt);

            RouteAttempt routeAttemptBranch = currentRouteAttempt.Clone(); ;
            TransportationItem.Node currentRoomNode = transportationItem.FindNodeByRoom(currentRouteAttempt.latestSegmentNode.room);
            TransportationItem.Node otherRoomNode = transportationItem.GetOtherNode(currentRoomNode);

            Debug.Log($"currentRoom: {currentRouteAttempt.latestSegmentNode.room}");
            Debug.Log($"otherRoom: {otherRoomNode.room}");

            if (!routeAttemptBranch.visitedRooms.Contains(otherRoomNode.room))
            // 
            {
                // Walk from the current location to the otherRoomEntrance
                routeAttemptBranch.GoTo(
                    new RouteSegment.Node(
                        currentRoomNode.cellCoordinates,
                        currentRoomNode.room
                    ),
                    RouteSegment.Type.WalkingAcrossRoom
                );

                // Use transportationItem and end up at the other Node
                routeAttemptBranch.GoTo(
                    new RouteSegment.Node(
                        otherRoomNode.cellCoordinates,
                        otherRoomNode.room
                    ),
                    RouteSegment.Type.UsingTransportationItem
                );

                // We are now in the other room, so we have visited it
                routeAttemptBranch.AddVisitedRoom(otherRoomNode.room);

                ContinueRouteAttempt(routeAttemptBranch);
            }
        }

        void CompleteRoute()
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
            currentRouteAttempt.status = RouteAttempt.Status.Complete;
            routeAttempts.Add(currentRouteAttempt);
        }
    }
}