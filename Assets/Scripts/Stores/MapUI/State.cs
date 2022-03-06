using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class State
    {
        public ToolState toolState { get; private set; }

        public delegate void ToolStateEvent(ToolState toolState, ToolState previousToolState);
        public ToolStateEvent onToolStateUpdated;

        public RoomKey selectedRoomKey { get; private set; }

        public delegate void SelectedRoomKeyEvent(RoomKey selectedRoomKey);
        public SelectedRoomKeyEvent onSelectedRoomKeyUpdated;

        public CellCoordinates buildStartCell { get; private set; } = null;
        public CellCoordinates currentSelectedCell { get; private set; } = null;

        public delegate void cellCoordinatesEvent(CellCoordinates currentSelectedCell);
        public cellCoordinatesEvent onCurrentSelectedCellUpdated;
        public cellCoordinatesEvent onBuildStartCellUpdated;

        public bool buildIsActive { get; private set; } = false;
        public delegate void buildIsActiveEvent(bool buildIsActive);
        public buildIsActiveEvent onBuildStart;
        public buildIsActiveEvent onBuildEnd;

        // this roomBlueprint is essentially just derived data, so no events needed
        public RoomBlueprint currentBlueprint { get; private set; }


        public State()
        {
            toolState = ToolState.None;
            selectedRoomKey = RoomKey.None;
            currentSelectedCell = CellCoordinates.zero;

            currentBlueprint = new RoomBlueprint(currentSelectedCell, selectedRoomKey);
        }

        public void SetToolState(ToolState toolState)
        {
            ToolState previousToolState = toolState;
            this.toolState = toolState;

            if (onToolStateUpdated != null)
            {
                onToolStateUpdated(toolState, previousToolState);
            }
        }

        public void SetSelectedRoomKey(RoomKey selectedRoomKey)
        {
            this.selectedRoomKey = selectedRoomKey;

            currentBlueprint.SetRoomKey(this.selectedRoomKey);
            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);

            if (onSelectedRoomKeyUpdated != null)
            {
                onSelectedRoomKeyUpdated(selectedRoomKey);
            }
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;

            if (buildIsActive)
            {
                currentBlueprint.SetBuildEndCell(currentSelectedCell);
            }
            else
            {
                currentBlueprint.SetBuildStartCell(currentSelectedCell);
            }

            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);

            if (onCurrentSelectedCellUpdated != null)
            {
                onCurrentSelectedCellUpdated(currentSelectedCell);
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

            AttemptToCreateRoomAtCurrentSelectedCell();

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

            if (currentSelectedCell == null)
            {
                return;
            }

            currentBlueprint.SetBuildStartCell(currentSelectedCell.Clone());

            if (onBuildStartCellUpdated != null)
            {
                onBuildStartCellUpdated(buildStartCell);
            }
        }

        // TODO - does this belong here or in Map Store?
        public void AttemptToCreateRoomAtCurrentSelectedCell()
        {
            if (selectedRoomKey == RoomKey.None)
            {
                return;
            }

            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);

            List<RoomBlueprintValidationError> validationErrors = currentBlueprint.GetAllValidationErrors();

            if (validationErrors.Count > 0)
            {
                // TODO - these should be unique messages
                foreach (RoomBlueprintValidationError validationError in validationErrors)
                {
                    Registry.Stores.Notifications.createNotification(validationError.message);
                }
                return;
            }

            MapRoom newRoom = new MapRoom(selectedRoomKey, currentBlueprint);
            Registry.Stores.Map.AddRoom(newRoom);

            MapRoomDetails roomDetails = Map.Constants.ROOM_DETAILS_MAP[selectedRoomKey];
            Registry.Stores.Wallet.SubtractBalance(roomDetails.price);
        }
    }
}

