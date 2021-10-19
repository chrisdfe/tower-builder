using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class CellCoordinates
    {
        public int x;
        public int z;
        public int floor;

        public CellCoordinates() { }

        public CellCoordinates(int x, int z, int floor)
        {
            this.x = x;
            this.z = z;
            this.floor = floor;
        }

        public override string ToString()
        {
            return $"({x}, {z}), floor {floor}";
        }

        public static bool Matches(CellCoordinates a, CellCoordinates b)
        {
            return (
                a.x == b.x &&
                a.z == b.z &&
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
                return new CellCoordinates(0, 0, 0);
            }
        }
    }
}


