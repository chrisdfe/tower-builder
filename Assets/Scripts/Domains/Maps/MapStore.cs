using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Domains;

namespace TowerBuilder.Domains.Map
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
