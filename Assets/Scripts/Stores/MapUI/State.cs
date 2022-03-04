using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class State
    {
        public ToolState toolState { get; private set; }
        public RoomKey selectedRoomKey { get; private set; }
        public CellCoordinates currentSelectedTile { get; private set; }
        public RoomBlueprint currentBlueprint { get; private set; }
        // public int currentFocusFloor;

        public delegate void ToolStateEvent(ToolState toolState, ToolState previousToolState);
        public ToolStateEvent onToolStateUpdated;

        public delegate void SelectedRoomKeyEvent(RoomKey selectedRoomKey);
        public SelectedRoomKeyEvent onSelectedRoomKeyUpdated;

        public delegate void CurrentSelectedTileEvent(CellCoordinates currentSelectedTile);
        public CurrentSelectedTileEvent onCurrentSelectedTileUpdated;


        public State()
        {
            toolState = ToolState.None;
            selectedRoomKey = Rooms.RoomKey.None;
            currentSelectedTile = CellCoordinates.zero;

            currentBlueprint = new RoomBlueprint(currentSelectedTile, selectedRoomKey);
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
            this.currentSelectedTile = currentSelectedCell;

            currentBlueprint.SetCellCoordinates(currentSelectedCell);
            currentBlueprint.Validate(Registry.Stores.Map.mapRooms, Registry.Stores.Wallet.balance);

            if (onCurrentSelectedTileUpdated != null)
            {
                onCurrentSelectedTileUpdated(currentSelectedCell);
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

            RoomDetails roomDetails = Rooms.Constants.ROOM_DETAILS_MAP[selectedRoomKey];
            Registry.Stores.Wallet.SubtractBalance(roomDetails.price);
        }
    }
}

