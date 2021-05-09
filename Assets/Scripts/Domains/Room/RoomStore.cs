using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Rooms
{
    public class RoomStore : StoreBase<RoomState>
    {
        public RoomStore()
        {
            state.roomKeyMap = new RoomKeyMap();
        }
    }
}
