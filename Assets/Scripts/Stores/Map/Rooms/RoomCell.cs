using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomCell
    {
        public Room room { get; private set; }
        public CellCoordinates coordinates;
        public List<RoomEntrance> entrances = new List<RoomEntrance>();

        public RoomCell(Room room)
        {
            this.room = room;
        }

        public void AddEntrance(RoomEntrance roomEntrance)
        {
            entrances.Add(roomEntrance);
            roomEntrance.roomCell = this;
        }
    }
}


