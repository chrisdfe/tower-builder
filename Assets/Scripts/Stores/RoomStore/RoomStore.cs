using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public class RoomStore : StoreBase<RoomState>
    {
        public RoomStore()
        {
            state.roomKeyMap = new RoomKeyMap();
        }
    }
}
