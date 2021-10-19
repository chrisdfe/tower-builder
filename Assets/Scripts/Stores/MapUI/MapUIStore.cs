using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.MapUI
{
    public partial class MapUIStore : StoreBase<MapUIState>
    {
        public MapUIStore()
        {
            state.toolState = ToolState.None;
            state.selectedRoomKey = Rooms.RoomKey.None;
            state.currentFocusFloor = 0;
            state.currentBlueprint = null;
            state.currentBlueprintRotation = MapRoomRotation.Right;
            state.currentSelectedTile = CellCoordinates.zero;
        }
    }
}
