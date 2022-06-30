using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace TowerBuilder.State.Rooms.Entrances
{
    public class RoomEntrance
    {
        private static int autoincrementingId;
        public int id { get; private set; }

        public RoomEntrancePosition position;
        public CellCoordinates cellCoordinates;

        public RoomEntrance()
        {
            GenerateId();
        }

        public override string ToString()
        {
            return $"Entrance {id}";
        }

        public RoomEntrance Clone()
        {
            return new RoomEntrance()
            {
                position = this.position,
                cellCoordinates = this.cellCoordinates
            };
        }

        private void GenerateId()
        {
            id = Interlocked.Increment(ref autoincrementingId);
        }
    }
}


