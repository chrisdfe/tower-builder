using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Blueprints;
using TowerBuilder.Stores.Rooms.Connections;
using TowerBuilder.Stores.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Stores.UI
{
    public class BuildToolState : ToolStateBase
    {
        public CellCoordinates buildStartCell { get; private set; } = null;
        public UI.State.cellCoordinatesEvent onBuildStartCellUpdated;

        public string selectedRoomCategory { get; private set; }
        public delegate void SelectedRoomCategoryEvent(string selectedRoomCategory);
        public SelectedRoomCategoryEvent onSelectedRoomCategoryUpdated;

        public RoomTemplate selectedRoomTemplate { get; private set; }
        public delegate void SelectedRoomTemplateEvent(RoomTemplate selectedRoomTemplate);
        public SelectedRoomTemplateEvent onSelectedRoomTemplateUpdated;

        public bool buildIsActive { get; private set; } = false;

        public delegate void buildIsActiveEvent(bool buildIsActive);
        public buildIsActiveEvent onBuildStart;
        public buildIsActiveEvent onBuildEnd;

        // this roomBlueprint is essentially just derived data, so no events needed
        public Blueprint currentBlueprint { get; private set; }

        public RoomConnections blueprintRoomConnections = new RoomConnections();
        public delegate void BlueprintRoomConnectionEvent(RoomConnections roomConnections);
        public BlueprintRoomConnectionEvent onBlueprintRoomConnectionsUpdated;

        public BuildToolState(UI.State state) : base(state) { }

        public override void Setup()
        {
            CreateBlueprint();
        }

        public override void Teardown()
        {
            DeleteBlueprint();
            ResetBlueprintRoomConnections();

            buildStartCell = null;
            buildIsActive = false;
        }

        public override void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            if (buildIsActive)
            {
                currentBlueprint.SetBuildEndCell(currentSelectedCell);
            }
            else
            {
                currentBlueprint.SetBuildStartCell(currentSelectedCell);
            }

            currentBlueprint.Validate(Registry.Stores);
            SearchForBlueprintRoomConnections();
        }

        public void SetSelectedRoomCategory(string roomCategory)
        {
            selectedRoomCategory = roomCategory;
            List<RoomTemplate> roomTemplates = Rooms.Constants.ROOM_DEFINITIONS.FindAll(details => details.category == roomCategory);

            RoomTemplate roomTemplate = roomTemplates[0];

            if (roomTemplate != null)
            {
                SelectRoomTemplateAndUpdateBlueprint(roomTemplate);
            }

            if (onSelectedRoomCategoryUpdated != null)
            {
                onSelectedRoomCategoryUpdated(roomCategory);
            }

            if (roomTemplate != null && onSelectedRoomTemplateUpdated != null)
            {
                onSelectedRoomTemplateUpdated(selectedRoomTemplate);
            }
        }

        public void SetSelectedRoomTemplate(RoomTemplate roomTemplate)
        {
            // TODO - put this somewhere more general
            // RoomTemplate roomTemplate = Rooms.Constants.ROOM_DEFINITIONS.Find(details => details.key == roomKey);

            SelectRoomTemplateAndUpdateBlueprint(roomTemplate);

            if (onSelectedRoomTemplateUpdated != null)
            {
                onSelectedRoomTemplateUpdated(selectedRoomTemplate);
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
            if (selectedRoomTemplate == null)
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
                blueprintRoomConnections.SearchForNewConnectionsToRoom(Registry.Stores.Rooms.rooms, currentBlueprint.room);

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

        void SelectRoomTemplateAndUpdateBlueprint(RoomTemplate roomTemplate)
        {
            this.selectedRoomTemplate = roomTemplate;

            currentBlueprint.SetRoomTemplate(this.selectedRoomTemplate);
            currentBlueprint.Validate(Registry.Stores);
        }

        void CreateBlueprint()
        {
            currentBlueprint = new Blueprint(selectedRoomTemplate, parentState.currentSelectedCell);
        }

        void DeleteBlueprint()
        {
            currentBlueprint.OnDestroy();
            currentBlueprint = null;
        }

        void AttemptToCreateRoomFromCurrentBlueprint()
        {
            if (selectedRoomTemplate == null)
            {
                return;
            }

            // TODO - should everything here on down go in MapState?
            List<RoomValidationError> validationErrors = currentBlueprint.Validate(Registry.Stores);

            if (validationErrors.Count > 0)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (RoomValidationError validationError in validationErrors)
                {
                    Registry.Stores.Notifications.createNotification(validationError.message);
                }
                return;
            }

            // 
            Registry.Stores.Wallet.SubtractBalance(currentBlueprint.room.GetPrice());

            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = FindRoomsToCombineWith(currentBlueprint.room);

            Room newRoom = currentBlueprint.room;
            if (roomsToCombineWith.Count > 0)
            {
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    newRoom.AddBlocks(otherRoom.blocks);

                    Registry.Stores.Rooms.DestroyRoom(otherRoom);
                }

                // TODO - add to the 1st item in roomsToCombineWith instead of replacing both with a new room?
            }

            Registry.Stores.Rooms.AddRoom(newRoom);

            SearchForBlueprintRoomConnections();

            if (blueprintRoomConnections.connections.Count > 0)
            {
                Registry.Stores.Rooms.AddRoomConnections(blueprintRoomConnections);
            }
            currentBlueprint.Reset();
        }

        List<Room> FindRoomsToCombineWith(Room room)
        {
            List<Room> result = new List<Room>();

            if (room.roomTemplate.resizability.Matches(RoomResizability.Inflexible()))
            {
                return result;
            }

            if (room.roomTemplate.resizability.x)
            {
                //  Check on either side
                foreach (int floor in room.roomCells.GetFloorRange())
                {
                    Room leftRoom = Registry.Stores.Rooms.rooms.FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetLowestX() - 1,
                        floor
                    ));

                    Room rightRoom = Registry.Stores.Rooms.rooms.FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetHighestX() + 1,
                        floor
                    ));

                    foreach (Room otherRoom in new Room[] { leftRoom, rightRoom })
                    {
                        if (
                            otherRoom != null &&
                            otherRoom.roomTemplate.key == selectedRoomTemplate.key &&
                            !result.Contains(otherRoom)
                        )
                        {
                            result.Add(otherRoom);
                        }
                    }
                }
            }

            if (room.roomTemplate.resizability.floor)
            {
                //  Check on floors above and below
                foreach (int x in room.roomCells.GetXRange())
                {
                    Room aboveRoom = Registry.Stores.Rooms.rooms.FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetHighestFloor() + 1
                    ));

                    Room belowRoom = Registry.Stores.Rooms.rooms.FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetLowestFloor() - 1
                    ));

                    foreach (Room otherRoom in new Room[] { aboveRoom, belowRoom })
                    {
                        if (
                            otherRoom != null &&
                            otherRoom.roomTemplate.key == selectedRoomTemplate.key &&
                            !result.Contains(otherRoom)
                        )
                        {
                            result.Add(otherRoom);
                        }
                    }
                }
            }

            return result;
        }
    }
}