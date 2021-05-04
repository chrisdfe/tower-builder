using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public class MapStore : StoreBase<MapState>
    {
        public MapStore()
        {
            state.roomCellsMap = new RoomCellsMap();
            state.roomGroupMap = new RoomGroupMap();

            // TODO - listen to room events here
        }
    }
}
