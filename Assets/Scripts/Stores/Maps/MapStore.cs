using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore : StoreBase<MapState>
    {
        public MapStore()
        {
            state.mapRooms = new List<MapRoom>();
        }
    }
}
