using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Validators.Entities;
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

        public List<RouteAttempt> sucessfulRouteAttempts =>
            routeAttempts.FindAll(routeAttempt => routeAttempt.status == RouteAttempt.Status.Complete);

        // TODO - find shortest
        public RouteAttempt bestRouteAttempt => sucessfulRouteAttempts.Count > 0 ? sucessfulRouteAttempts[0] : null;

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

            startRoom = appState.Entities.Rooms.queries.FindRoomAtCell(startCoordinates);
            endRoom = appState.Entities.Rooms.queries.FindRoomAtCell(endCoordinates);
            routeAttempts = new List<RouteAttempt>();

            RouteAttempt firstRouteAttempt = new RouteAttempt();
            firstRouteAttempt.currentCellCoordinates = startCoordinates;
            firstRouteAttempt.currentRoom = startRoom;
            firstRouteAttempt.AddVisitedRoom(startRoom);
            routeAttempts.Add(firstRouteAttempt);

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
                if (!GenericEntityValidations.IsValidStandardLocation(appState, cellCoordinates))
                {
                    errors.Add(new RouteError($"Invalid coordinates: {cellCoordinates}"));
                }
            }
        }

        /* 
            Internals
        */
        void ContinueRouteAttempt(RouteAttempt currentRouteAttempt)
        {
            // TODO - make sure the resident is on the same floor as the end point too
            if (CanWalkTo(currentRouteAttempt, endCoordinates))
            {
                CompleteRoute(currentRouteAttempt);
            }
            else
            {
                ListWrapper<TransportationItem> transportationItemsList =
                    appState.Entities.TransportationItems.queries
                        .FindTransportationItemsEnterableFromRoom(currentRouteAttempt.currentRoom);

                transportationItemsList.ForEach(transportationItem =>
                {
                    UseTransportationItemIfUnvisited(currentRouteAttempt, transportationItem);
                });
            }
        }

        void UseTransportationItemIfUnvisited(RouteAttempt currentRouteAttempt, TransportationItem transportationItem)
        {
            var (entranceCoordinatesList, exitCoordinatesList) = GetValidTransportationItemEntrancesAndExits(transportationItem, currentRouteAttempt);

            if (entranceCoordinatesList.Count == 0 || exitCoordinatesList.Count == 0)
            {
                // We have reached the end of this route
                return;
            }

            foreach (CellCoordinates entranceCoordinates in entranceCoordinatesList.items)
            {
                foreach (CellCoordinates exitCoordinates in exitCoordinatesList.items)
                {
                    Room nextRoom = Registry.appState.Entities.Rooms.queries.FindRoomAtCell(exitCoordinates);

                    if (!currentRouteAttempt.visitedRooms.Contains(nextRoom) && CanWalkTo(currentRouteAttempt, entranceCoordinates))
                    {
                        RouteAttempt routeAttemptBranch = currentRouteAttempt.Clone();
                        routeAttempts.Add(routeAttemptBranch);

                        // TODO next - draw a path between current coordinates and entrance coordinates
                        //             and make sure each step along the way is valid (has floor, enough clearance etc)

                        // Walk from the current location to the otherRoomEntrance
                        routeAttemptBranch.GoTo(
                            new RouteSegment.Node(
                                entranceCoordinates,
                                routeAttemptBranch.currentRoom
                            ),
                            RouteSegment.Type.WalkingAcrossRoom
                        );

                        // Use transportationItem and end up at the other Node in the other room
                        routeAttemptBranch.GoTo(
                            new RouteSegment.Node(
                                exitCoordinates,
                                nextRoom
                            ),
                            RouteSegment.Type.UsingTransportationItem
                        );

                        ContinueRouteAttempt(routeAttemptBranch);
                    }
                }
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

        bool IsOnLatestSegmentFloor(RouteAttempt routeAttempt, CellCoordinates cellCoordinates) =>
            cellCoordinates.floor == routeAttempt.latestSegmentNode.cellCoordinates.floor;

        bool IsInCurrentRoom(RouteAttempt routeAttempt, CellCoordinates cellCoordinates) =>
                Registry.appState.Entities.Rooms.queries
                    .FindRoomAtCell(cellCoordinates) == routeAttempt.currentRoom;

        bool CanWalkTo(RouteAttempt routeAttempt, CellCoordinates targetCellCoordinates)
        {
            if (appState.Entities.Rooms.queries.FindEntityTypeAtCell(targetCellCoordinates) != routeAttempt.currentRoom)
            {
                return false;
            }

            // At this point I'm assuming targetCellCoordinates and currentCoordinates are on the same floor
            int currentX = routeAttempt.latestSegmentNode.cellCoordinates.x;

            // TODO - factor in entity width
            while (currentX != targetCellCoordinates.x)
            {
                if (currentX > targetCellCoordinates.x)
                {
                    currentX--;
                }
                else if (currentX < targetCellCoordinates.x)
                {
                    currentX++;
                }

                CellCoordinates currentCoordinates = new CellCoordinates(currentX, targetCellCoordinates.floor);

                if (!GenericEntityValidations.IsValidStandardLocation(appState, currentCoordinates))
                {
                    return false;
                }
            }

            return true;
        }

        (CellCoordinatesList, CellCoordinatesList) GetValidTransportationItemEntrancesAndExits(TransportationItem transportationItem, RouteAttempt routeAttempt)
        {
            CellCoordinatesList validEntrances =
                new CellCoordinatesList(
                    transportationItem.entranceCellCoordinatesList.items
                        .FindAll((entranceCellCoordinates) =>
                            GenericEntityValidations.IsValidStandardLocation(appState, entranceCellCoordinates) &&
                            IsOnLatestSegmentFloor(routeAttempt, entranceCellCoordinates) &&
                            IsInCurrentRoom(routeAttempt, entranceCellCoordinates)
                        )
                        .ToList()
                );
            CellCoordinatesList validExits =
                new CellCoordinatesList(
                    transportationItem.exitCellCoordinatesList.items
                        .FindAll((exitCellCoordinates) =>
                            GenericEntityValidations.IsValidStandardLocation(appState, exitCellCoordinates) &&
                            !IsInCurrentRoom(routeAttempt, exitCellCoordinates)
                        )
                        .ToList()
                );

            return (validEntrances, validExits);
        }
    }
}