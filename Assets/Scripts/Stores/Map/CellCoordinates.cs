using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class CellCoordinates
    {
        public int x;
        public int floor;

        public CellCoordinates(int x, int floor)
        {
            this.x = x;
            this.floor = floor;
        }

        public override string ToString()
        {
            return $"x: {x}, floor: {floor}";
        }

        public CellCoordinates Add(CellCoordinates b)
        {
            return CellCoordinates.Add(this, b);
        }

        public CellCoordinates Subtract(CellCoordinates b)
        {
            return CellCoordinates.Subtract(this, b);
        }

        public bool Matches(CellCoordinates b)
        {
            return CellCoordinates.Matches(this, b);
        }

        public static CellCoordinates Add(CellCoordinates a, CellCoordinates b)
        {
            return new CellCoordinates(a.x + b.x, a.floor + b.floor);
        }

        public static CellCoordinates Subtract(CellCoordinates a, CellCoordinates b)
        {
            return new CellCoordinates(a.x - b.x, a.floor - b.floor);
        }

        public static bool Matches(CellCoordinates a, CellCoordinates b)
        {
            return (
                a.x == b.x &&
                a.floor == b.floor
            );
        }

        public CellCoordinates Clone()
        {
            return new CellCoordinates(x, floor);
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


