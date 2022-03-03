using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class CellCoordinates
    {
        public int x = 0;
        // public int z = 0;
        public int floor = 0;

        public CellCoordinates() { }

        public CellCoordinates(int x, int floor)
        {
            this.x = x;
            // this.z = z;
            this.floor = floor;
        }

        public override string ToString()
        {
            return $"column {x}, floor {floor}";
        }

        public static bool Matches(CellCoordinates a, CellCoordinates b)
        {
            return (
                a.x == b.x &&
                // a.z == b.z &&
                a.floor == b.floor
            );
        }

        public bool Matches(CellCoordinates b)
        {
            return CellCoordinates.Matches(this, b);
        }

        public static CellCoordinates zero
        {
            get
            {
                return new CellCoordinates(0, 0);
            }
        }
    }
}


