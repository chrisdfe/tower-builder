using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace TowerBuilder.DataTypes.Rooms.Entrances
{
    public class RoomEntrance
    {
        public enum Position
        {
            Left,
            Right
        }

        public int id { get; private set; }

        public Position position;
        public CellCoordinates cellCoordinates;

        public RoomEntrance()
        {
            this.id = UIDGenerator.Generate("TransportationItem");
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
    }
}


