using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class Room
    {
        public string id { get; private set; }
        public RoomKey roomKey { get; private set; }

        public RoomCells roomCells { get; private set; }
        public List<RoomEntrance> roomEntrances { get; private set; }

        public Room(RoomKey roomKey, Blueprint blueprint)
        {
            id = GenerateId();
            this.roomKey = roomKey;

            roomCells = blueprint.GetRoomCells();
            RoomDetails roomDetails = GetDetails();

            // RoomEntrances need a Room instance attached to them to work properly
            roomEntrances = new List<RoomEntrance>(roomDetails.entrances);

            foreach (RoomEntrance roomEntrance in roomEntrances)
            {
                roomEntrance.room = this;
            }
        }

        public RoomDetails GetDetails()
        {
            return Room.GetDetails(roomKey);
        }

        public static RoomDetails GetDetails(RoomKey roomKey)
        {
            return Rooms.Constants.ROOM_DETAILS_MAP[roomKey];
        }

        string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}


