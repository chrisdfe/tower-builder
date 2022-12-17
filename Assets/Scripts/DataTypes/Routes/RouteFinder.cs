using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
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

            Debug.Log("errors");
            Debug.Log(errors);

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
                Debug.Log("is valid inside point: " + IsValidInsideRoutePoint(cellCoordinates));
                Debug.Log("is valid outside point: " + IsValidOutsideRoutePoint(cellCoordinates));

                // TODO - put these into a more general functions
                if (
                    !IsValidInsideRoutePoint(cellCoordinates) &&
                    !IsValidOutsideRoutePoint(cellCoordinates)
                )
                {
                    errors.Add(new RouteError($"Invalid coordinates: {cellCoordinates}"));
                }
            }
        }

        /* 
            Internals
        */
        // TODO - put these two somewhere more general so they can be used for resident validation too
        // outside + on the ground
        bool IsValidOutsideRoutePoint(CellCoordinates cellCoordinates) =>
            appState.Entities.Rooms.queries.FindEntityTypeAtCell(cellCoordinates) == null &&
            cellCoordinates.floor == 0;

        // inside + on a floor
        bool IsValidInsideRoutePoint(CellCoordinates cellCoordinates) =>
            appState.Entities.Rooms.queries.FindEntityTypeAtCell(cellCoordinates) != null &&
            appState.Entities.Floors.queries.FindEntityTypeAtCell(cellCoordinates) != null;

        void ContinueRouteAttempt(RouteAttempt currentRouteAttempt)
        {
            if (currentRouteAttempt.latestSegmentNode.room == endRoom)
            {
                CompleteRoute(currentRouteAttempt);
            }
            else
            {
                ListWrapper<TransportationItem> transportationItemsList =
                    appState.Entities.TransportationItems.queries
                        .FindTransportationItemsEnterableFromRoom(currentRouteAttempt.currentRoom)
                        // find transportation items that have an entrance on the current floor
                        // .FindAll((transportationItem) => (
                        //     transportationItem.entranceCellCoordinatesList.Find(
                        //         (entranceCellCoordinates) => (
                        //             appState
                        //                 .Entities.Floors.queries
                        //                 .FindEntityTypeAtCell(entranceCellCoordinates) != null
                        //         )
                        //     ) != null
                        // ));
                        ;

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

            CellCoordinates entranceCoordinates =
                transportationItem.entranceCellCoordinatesList.items.Find((cellCoordinates) =>
                {
                    Room room = Registry.appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates);
                    return room == routeAttemptBranch.currentRoom;
                });

            CellCoordinatesList exitCoordinatesList = new CellCoordinatesList(
                transportationItem.exitCellCoordinatesList.items
                    // No need to pay attention to  exits that exit into the current room
                    .FindAll((cellCoordinates) =>
                    {
                        Room room = Registry.appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates);
                        return room != routeAttemptBranch.currentRoom;
                    })
            );

            foreach (CellCoordinates exitCoordinates in exitCoordinatesList.items)
            {
                // TODO - check if transportation item is one way
                Room nextRoom = Registry.appState.Entities.Rooms.queries.FindRoomAtCell(exitCoordinates);

                if (!routeAttemptBranch.visitedRooms.Contains(nextRoom))
                {
                    // Walk from the current location to the otherRoomEntrance
                    routeAttemptBranch.GoTo(
                        new RouteSegment.Node(
                            entranceCoordinates,
                            routeAttemptBranch.currentRoom
                        ),
                        RouteSegment.Type.WalkingAcrossRoom
                    );

                    // Use transportationItem and end up at the other Node
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