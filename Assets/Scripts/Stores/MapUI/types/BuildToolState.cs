using System.Collections.Generic;
using TowerBuilder.Stores.Map;
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
        public RoomBlueprint currentBlueprint { get; private set; }

        public BuildToolState(MapUI.State state) : base(state)
        {
            Reset();
        }

        public override void OnCurrentSelectedCellSet()
        {
            if (buildIsActive)
            {
                currentBlueprint.SetBuildEndCell(parentState.currentSelectedCell);
            }
            else
            {
                currentBlueprint.SetBuildStartCell(parentState.currentSelectedCell);
            }

            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);
        }

        public override void Reset()
        {
            this.selectedRoomKey = RoomKey.None;
            currentBlueprint = new RoomBlueprint(parentState.currentSelectedCell, selectedRoomKey);
        }

        public void SetSelectedRoomKey(RoomKey roomKey)
        {
            this.selectedRoomKey = roomKey;

            currentBlueprint.SetRoomKey(this.selectedRoomKey);
            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);

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

        void AttemptToCreateRoomAtCurrentSelectedCell()
        {
            if (selectedRoomKey == RoomKey.None)
            {
                return;
            }

            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);

            List<RoomBlueprintValidationError> validationErrors = currentBlueprint.GetAllValidationErrors();

            if (validationErrors.Count > 0)
            {
                // TODO - these should be unique messages - right now they 
                foreach (RoomBlueprintValidationError validationError in validationErrors)
                {
                    Registry.Stores.Notifications.createNotification(validationError.message);
                }
                return;
            }

            Room newRoom = new Room(selectedRoomKey, currentBlueprint);
            Registry.Stores.Map.AddRoom(newRoom);

            MapRoomDetails roomDetails = Map.Constants.ROOM_DETAILS_MAP[selectedRoomKey];
            Registry.Stores.Wallet.SubtractBalance(roomDetails.price);
        }
    }
}