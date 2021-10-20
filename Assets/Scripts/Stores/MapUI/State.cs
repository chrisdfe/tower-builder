using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.MapUI
{
    public class State
    {
        public ToolState toolState { get; private set; }
        public RoomKey selectedRoomKey { get; private set; }
        public CellCoordinates currentSelectedTile { get; private set; }
        public RoomBlueprint currentBlueprint { get; private set; }
        public MapRoomRotation currentBlueprintRotation { get; private set; }
        public int currentFocusFloor;

        public delegate void ToolStateEvent(ToolState toolState, ToolState previousToolState);
        public ToolStateEvent onToolStateUpdated;

        public delegate void SelectedRoomKeyEvent(RoomKey selectedRoomKey);
        public SelectedRoomKeyEvent onSelectedRoomKeyUpdated;

        public delegate void CurrentFocusFloorEvent(int currentFocusFloor);
        public CurrentFocusFloorEvent onCurrentFocusFloorUpdated;

        public delegate void CurrentSelectedTileEvent(CellCoordinates currentSelectedTile);
        public CurrentSelectedTileEvent onCurrentSelectedTileUpdated;

        public delegate void BlueprintRotationEvent(MapRoomRotation rotation);
        public BlueprintRotationEvent onBlueprintRotationUpdated;

        public State()
        {
            toolState = ToolState.None;
            selectedRoomKey = Rooms.RoomKey.None;
            currentFocusFloor = 0;
            currentBlueprintRotation = MapRoomRotation.Right;
            currentSelectedTile = CellCoordinates.zero;
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

            if (onSelectedRoomKeyUpdated != null)
            {
                onSelectedRoomKeyUpdated(selectedRoomKey);
            }
        }

        public void SetCurrentFocusFloor(int currentFocusFloor)
        {
            this.currentFocusFloor = currentFocusFloor;

            if (onCurrentFocusFloorUpdated != null)
            {
                onCurrentFocusFloorUpdated(currentFocusFloor);
            }
        }

        public void FocusFloorUp()
        {
            SetCurrentFocusFloor(currentFocusFloor + 1);
        }

        public void FocusFloorDown()
        {
            SetCurrentFocusFloor(currentFocusFloor - 1);
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedTile)
        {
            this.currentSelectedTile = currentSelectedTile;

            if (onCurrentSelectedTileUpdated != null)
            {
                onCurrentSelectedTileUpdated(currentSelectedTile);
            }
        }

        public void SetCurrentBlueprintRotation(MapRoomRotation rotation)
        {
            currentBlueprintRotation = rotation;

            if (onBlueprintRotationUpdated != null)
            {
                onBlueprintRotationUpdated(rotation);
            }
        }
    }
}

