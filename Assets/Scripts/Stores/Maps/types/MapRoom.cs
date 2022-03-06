using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.Map
{
    public class MapRoom
    {
        public string id { get; private set; }
        public RoomKey roomKey { get; private set; }
        public List<CellCoordinates> roomCells { get; private set; }

        public MapRoom(RoomKey roomKey, RoomBlueprint blueprint)
        {
            id = GenerateId();
            this.roomKey = roomKey;
            roomCells = blueprint.GetRoomCells();
        }

        string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}


