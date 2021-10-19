using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class CellCoordinates2D
    {
        public int x;
        public int z;

        public CellCoordinates2D() { }

        public CellCoordinates2D(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({x}, {z})";
        }

        public static bool Matches(CellCoordinates2D a, CellCoordinates2D b)
        {
            return (
                a.x == b.x &&
                a.z == b.z
            );
        }

        public bool Matches(CellCoordinates2D b)
        {
            return CellCoordinates2D.Matches(this, b);
        }

        public static CellCoordinates2D zero
        {
            get
            {
                return new CellCoordinates2D(0, 0);
            }
        }
    }
}


