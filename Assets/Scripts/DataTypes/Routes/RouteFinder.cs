using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class RouteFinder
    {
        public List<RouteAttempt> routeAttempts = new List<RouteAttempt>();

        CellCoordinates startCoordinates;
        CellCoordinates endCoordinates;

        Room startRoom;
        Room endRoom;

        public List<RouteFinderError> errors = new List<RouteFinderError>();

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

        public Route FindRouteBetween(CellCoordinates startCoordinates, CellCoordinates endCoordinates)
        {
            ValidateRouteMarker(startCoordinates);
            ValidateRouteMarker(endCoordinates);

            if (errors.Count > 0)
            {
                return null;
            }

            this.startCoordinates = startCoordinates;
            this.endCoordinates = endCoordinates;

            startRoom = Registry.appState.Rooms.queries.FindRoomAtCell(startCoordinates);
            endRoom = Registry.appState.Rooms.queries.FindRoomAtCell(endCoordinates);

            BeginFirstRouteAttempt();

            if (bestRouteAttempt != null)
            {
                return new Route(bestRouteAttempt);
            }

            return null;

            void BeginFirstRouteAttempt()
            {
                routeAttempts = new List<RouteAttempt>();

                RouteAttempt firstRouteAttempt = new RouteAttempt(new List<Room>(), new List<RouteSegment>());

                firstRouteAttempt.AddVisitedRoom(startRoom);

                ContinueRouteAttempt(firstRouteAttempt);
            }

            void ValidateRouteMarker(CellCoordinates cellCoordinates)
            {
                Room room = Registry.appState.Rooms.queries.FindRoomAtCell(cellCoordinates);

                // The 0th floor is a special case, since it connects to the outside world
                // if (room == null && cellCoordinates.floor != 0)
                // For now both ends have to be in a room
                if (room == null)
                {
                    errors.Add(new RouteFinderError($"Invalid coordinates: {cellCoordinates}"));
                }
            }
        }


        void ContinueRouteAttempt(RouteAttempt currentRouteAttempt)
        {
            RouteSegment.Node latestSegmentNode;
            if (currentRouteAttempt.routeSegments.Count == 0)
            {
                latestSegmentNode = new RouteSegment.Node(startCoordinates, startRoom);
            }
            else
            {
                latestSegmentNode = currentRouteAttempt.GetLatestSegmentNode();
            }

            Room currentRoom = latestSegmentNode.room;

            if (currentRoom == endRoom)
            {
                CompleteRoute();
            }
            else
            {
                ContinueRouteSearch();
            }

            void ContinueRouteSearch()
            {
                // Keep searching for connections
                RoomConnectionList roomConnectionsList = Registry.appState.Rooms.roomConnectionList.FindConnectionsForRoom(currentRoom);

                foreach (RoomConnection roomConnection in roomConnectionsList.items)
                {
                    RouteAttempt routeAttemptBranch = BranchRouteAttempt(currentRouteAttempt);
                    Room otherRoom = roomConnection.GetConnectedRoom(currentRoom);

                    if (!routeAttemptBranch.visitedRooms.Contains(otherRoom))
                    // 
                    {
                        // The entrance to the other room in the current room
                        RoomConnection.Node connectionToOtherRoom = roomConnection.GetConnectionFor(currentRoom);
                        RoomEntrance currentRoomEntranceToOtherRoom = connectionToOtherRoom.roomEntrance;

                        // The entrance to the other room in the other room
                        RoomEntrance otherRoomEntrance = roomConnection.GetConnectedRoomEntrance(currentRoom);

                        RouteSegment.Node currentRoomEntranceToOtherRoomNode = new RouteSegment.Node(
                            currentRoomEntranceToOtherRoom.cellCoordinates,
                            currentRoom
                        );

                        // Walk from the current location to the otherRoomEntrance
                        routeAttemptBranch.AddRouteSegment(
                            new RouteSegment(
                                latestSegmentNode,
                                currentRoomEntranceToOtherRoomNode,
                                RouteSegment.Type.WalkingAcrossRoom
                            )
                        );

                        // Walk through the RoomConnection into the otherRoom
                        routeAttemptBranch.AddRouteSegment(
                            new RouteSegment(
                                currentRoomEntranceToOtherRoomNode,
                                new RouteSegment.Node(
                                    otherRoomEntrance.cellCoordinates,
                                    otherRoom
                                ),
                                RouteSegment.Type.UsingRoomConnection
                            )
                        );

                        // We are now in the other room, so we have visited it
                        routeAttemptBranch.AddVisitedRoom(otherRoom);

                        ContinueRouteAttempt(routeAttemptBranch);
                    }
                }
            }

            void CompleteRoute()
            {
                // Add final segment from entrance to the point in room
                currentRouteAttempt.AddRouteSegment(
                    new RouteSegment(
                        latestSegmentNode,
                        new RouteSegment.Node(
                            endCoordinates,
                            endRoom
                        ),
                        RouteSegment.Type.WalkingAcrossRoom
                    )
                );

                // The route is now over
                currentRouteAttempt.status = RouteAttempt.Status.Complete;
                routeAttempts.Add(currentRouteAttempt);
            }
        }

        RouteAttempt BranchRouteAttempt(RouteAttempt routeAttempt)
        {
            routeAttempts.Add(routeAttempt);
            return routeAttempt.Clone();
        }
    }
}