using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.State.Rooms.Entrances
{
    public class DefaultEntranceBuilder : RoomEntranceBuilderBase
    {
        public override List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            return new List<RoomEntrance>();
        }
    }
}