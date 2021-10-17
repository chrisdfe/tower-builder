using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;

namespace TowerBuilder.Stores.MapUI
{
    public partial class MapUIStore : StoreBase<MapUIState>
    {
        public MapUIStore()
        {
            state.toolState = ToolState.None;
            state.selectedRoomKey = Rooms.RoomKey.None;
            state.currentFocusFloor = 0;
        }
    }
}
