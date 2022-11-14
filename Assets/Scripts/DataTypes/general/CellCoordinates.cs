using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [System.Serializable]
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

        public CellCoordinates Clone()
        {
            return new CellCoordinates(x, floor);
        }

        public CellCoordinates GetCoordinatesAbove()
        {
            return new CellCoordinates(x, floor + 1);
        }

        public CellCoordinates GetCoordinatesBelow()
        {
            return new CellCoordinates(x, floor - 1);
        }

        public CellCoordinates GetCoordinatesLeft()
        {
            return new CellCoordinates(x - 1, floor);
        }

        public CellCoordinates GetCoordinatesRight()
        {
            return new CellCoordinates(x + 1, floor);
        }

        /* 
            Static API
        */
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

        public static CellCoordinates zero
        {
            get
            {
                return new CellCoordinates(0, 0);
            }
        }
    }
}


