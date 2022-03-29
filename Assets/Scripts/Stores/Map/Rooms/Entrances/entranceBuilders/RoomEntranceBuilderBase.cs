using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map.Rooms.Entrances
{
    public abstract class RoomEntranceBuilderBase
    {
        public abstract List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells);
    }
}