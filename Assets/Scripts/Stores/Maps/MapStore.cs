using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore : StoreBase<MapState>
    {
        public MapStore()
        {
            state.roomCellsMap = new RoomCellsMap();
            state.roomGroupMap = new RoomGroupMap();

            // TODO - listen to room events here
        }
    }
}
