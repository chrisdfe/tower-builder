using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class BuildToolState : ToolStateBase
    {
        public CellCoordinates buildStartCell { get; private set; } = null;
        public MapUI.State.cellCoordinatesEvent onBuildStartCellUpdated;

        public RoomKey selectedRoomKey { get; private set; }
        public delegate void SelectedRoomKeyEvent(RoomKey selectedRoomKey);
        public SelectedRoomKeyEvent onSelectedRoomKeyUpdated;

        public bool buildIsActive { get; private set; } = false;

        public delegate void buildIsActiveEvent(bool buildIsActive);
        public buildIsActiveEvent onBuildStart;
        public buildIsActiveEvent onBuildEnd;

        // this roomBlueprint is essentially just derived data, so no events needed
        public Blueprint currentBlueprint { get; private set; }

        public RoomConnections blueprintRoomConnections = new RoomConnections();
        public delegate void BlueprintRoomConnectionEvent(RoomConnections roomConnections);
        public BlueprintRoomConnectionEvent onBlueprintRoomConnectionsUpdated;

        public BuildToolState(MapUI.State state) : base(state) { }

        public override void Setup()
        {
            CreateBlueprint();

            parentState.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
        }

        public override void Teardown()
        {
            DeleteBlueprint();
            ResetBlueprintRoomConnections();

            buildStartCell = null;
            buildIsActive = false;

            parentState.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
        }

        public void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            if (buildIsActive)
            {
                currentBlueprint.SetBuildEndCell(currentSelectedCell);
            }
            else
            {
                currentBlueprint.SetBuildStartCell(currentSelectedCell);
            }


            currentBlueprint.Validate(Registry.Stores.Map.rooms, Registry.Stores.Wallet.balance);
            SearchForBlueprintRoomConnections();
        }


        public void SetSelectedRoomKey(RoomKey roomKey)
        {
            this.selectedRoomKey = roomKey;

            currentBlueprint.SetRoomKey(this.selectedRoomKey);
            currentBlueprint.Validate(Registry.Stores.Map.rooms, Registry.Stores.Wallet.balance);

            if (onSelectedRoomKeyUpdated != null)
            {
                onSelectedRoomKeyUpdated(selectedRoomKey);
            }
        }

        public void StartBuild()
        {
            buildIsActive = true;

            SetBuildStartCell();

            if (onBuildStart != null)
            {
                onBuildStart(buildIsActive);
            }
        }

        public void EndBuild()
        {
            buildIsActive = false;

            AttemptToCreateRoomFromCurrentBlueprint();

            if (onBuildEnd != null)
            {
                onBuildEnd(buildIsActive);
            }
        }

        public void SetBuildStartCell()
        {
            if (selectedRoomKey == RoomKey.None)
            {
                return;
            }

            if (parentState.currentSelectedCell == null)
            {
                return;
            }

            currentBlueprint.SetBuildStartCell(parentState.currentSelectedCell.Clone());

            if (onBuildStartCellUpdated != null)
            {
                onBuildStartCellUpdated(buildStartCell);
            }
        }


        public void SearchForBlueprintRoomConnections()
        {
            if (!currentBlueprint.IsValid())
            {
                ResetBlueprintRoomConnections();
                return;
            }

            RoomConnections newBlueprintConnections =
                blueprintRoomConnections.SearchForConnectionsToRoom(Registry.Stores.Map.rooms, currentBlueprint.room);

            this.blueprintRoomConnections = newBlueprintConnections;

            if (onBlueprintRoomConnectionsUpdated != null)
            {
                onBlueprintRoomConnectionsUpdated(this.blueprintRoomConnections);
            }
        }

        public void ResetBlueprintRoomConnections()
        {
            this.blueprintRoomConnections = new RoomConnections();

            if (onBlueprintRoomConnectionsUpdated != null)
            {
                onBlueprintRoomConnectionsUpdated(this.blueprintRoomConnections);
            }
        }

        void CreateBlueprint()
        {
            currentBlueprint = new Blueprint(selectedRoomKey, parentState.currentSelectedCell);
        }

        void DeleteBlueprint()
        {
            currentBlueprint.OnDestroy();
            currentBlueprint = null;
        }

        void AttemptToCreateRoomFromCurrentBlueprint()
        {
            if (selectedRoomKey == RoomKey.None)
            {
                return;
            }

            // TODO - should everything here on down go in MapState?
            List<BlueprintValidationError> validationErrors = currentBlueprint.Validate(Registry.Stores.Map.rooms, Registry.Stores.Wallet.balance);

            if (validationErrors.Count > 0)
            {
                // TODO - these should be unique messages - right now they are wnot
                foreach (BlueprintValidationError validationError in validationErrors)
                {
                    Registry.Stores.Notifications.createNotification(validationError.message);
                }
                return;
            }

            RoomDetails roomDetails = currentBlueprint.room.roomDetails;
            RoomCells roomCells = currentBlueprint.room.roomCells;

            // 
            Registry.Stores.Wallet.SubtractBalance(currentBlueprint.GetPrice());

            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = new List<Room>();

            if (!roomDetails.resizability.Matches(RoomResizability.Inflexible()))
            {
                if (roomDetails.resizability.x)
                {
                    //  Check on either side
                    foreach (int floor in roomCells.GetFloorRange())
                    {
                        Room leftRoom = Registry.Stores.Map.rooms.FindRoomAtCell(new CellCoordinates(
                            roomCells.GetLowestX() - 1,
                            floor
                        ));

                        Room rightRoom = Registry.Stores.Map.rooms.FindRoomAtCell(new CellCoordinates(
                            roomCells.GetHighestX() + 1,
                            floor
                        ));

                        foreach (Room otherRoom in new Room[] { leftRoom, rightRoom })
                        {
                            if (
                                otherRoom != null &&
                                otherRoom.roomKey == selectedRoomKey &&
                                !roomsToCombineWith.Contains(otherRoom)
                            )
                            {
                                roomsToCombineWith.Add(otherRoom);
                            }
                        }
                    }
                }

                if (roomDetails.resizability.floor)
                {
                    //  Check on floors above and below
                    foreach (int x in roomCells.GetXRange())
                    {
                        Room aboveRoom = Registry.Stores.Map.rooms.FindRoomAtCell(new CellCoordinates(
                            x,
                            roomCells.GetHighestFloor() + 1
                        ));

                        Room belowRoom = Registry.Stores.Map.rooms.FindRoomAtCell(new CellCoordinates(
                            x,
                            roomCells.GetLowestFloor() - 1
                        ));

                        foreach (Room otherRoom in new Room[] { aboveRoom, belowRoom })
                        {
                            if (
                                otherRoom != null &&
                                otherRoom.roomKey == selectedRoomKey &&
                                !roomsToCombineWith.Contains(otherRoom)
                            )
                            {
                                roomsToCombineWith.Add(otherRoom);
                            }
                        }
                    }
                }
            }

            Room newRoom = currentBlueprint.room;
            if (roomsToCombineWith.Count > 0)
            {
                RoomCells newRoomCells = currentBlueprint.room.roomCells;
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    // Group all of those roomcells into a single list
                    newRoomCells.Add(otherRoom.roomCells);

                    // Delete room
                    Registry.Stores.Map.DestroyRoom(otherRoom);
                }

                // Create new room with all of those room cells from previous rooms
                // TODO - add to the 1st item in roomsToCombineWith instead of replacing both with a new room?
                newRoom.SetRoomCells(newRoomCells);
            }

            Registry.Stores.Map.AddRoom(newRoom);

            currentBlueprint.Reset();
        }
    }
}