using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class MapRoom
    {
        public string id { get; private set; }
        public RoomKey roomKey { get; private set; }
        public RoomCells roomCells { get; private set; }

        public MapRoom(RoomBlueprint blueprint)
        {
            GenerateId();
            roomKey = blueprint.roomKey;
            roomCells = blueprint.GetPositionedRoomCells();
        }

        void GenerateId()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}


