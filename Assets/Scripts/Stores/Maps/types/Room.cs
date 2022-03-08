using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.Map
{
    public class Room
    {
        public string id { get; private set; }
        public RoomKey roomKey { get; private set; }
        public List<CellCoordinates> roomCells { get; private set; }

        public Room(RoomKey roomKey, RoomBlueprint blueprint)
        {
            id = GenerateId();
            this.roomKey = roomKey;
            roomCells = blueprint.GetRoomCells();
        }

        public MapRoomDetails GetDetails()
        {
            return Room.GetDetails(roomKey);
        }


        string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public static MapRoomDetails GetDetails(RoomKey roomKey)
        {
            return Stores.Map.Constants.ROOM_DETAILS_MAP[roomKey];
        }
    }
}


