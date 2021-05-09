using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Rooms
{
    public partial class RoomStore : StoreBase<RoomState>
    {
        public RoomStore()
        {
            state.roomKeyMap = new RoomKeyMap();
        }
    }
}
