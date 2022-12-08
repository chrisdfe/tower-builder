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

        public CellCoordinates coordinatesAbove => new CellCoordinates(x, floor + 1);

        public CellCoordinates coordinatesAboveRight => new CellCoordinates(x + 1, floor + 1);

        public CellCoordinates coordinatesRight => new CellCoordinates(x + 1, floor);

        public CellCoordinates coordinatesBelowRight => new CellCoordinates(x + 1, floor - 1);

        public CellCoordinates coordinatesBelow => new CellCoordinates(x, floor - 1);

        public CellCoordinates coordinatesBelowLeft => new CellCoordinates(x - 1, floor - 1);

        public CellCoordinates coordinatesLeft => new CellCoordinates(x - 1, floor);

        public CellCoordinates coordinatesAboveLeft => new CellCoordinates(x - 1, floor + 1);

        public CellCoordinates(int x, int floor)
        {
            this.x = x;
            this.floor = floor;
        }

        public override string ToString() => $"x: {x}, floor: {floor}";

        public CellCoordinates Add(CellCoordinates b) => CellCoordinates.Add(this, b);

        public CellCoordinates Subtract(CellCoordinates b) => CellCoordinates.Subtract(this, b);

        public bool Matches(CellCoordinates b) => CellCoordinates.Matches(this, b);

        public CellCoordinates Clone() => new CellCoordinates(x, floor);

        /* 
            Static API
        */
        public static CellCoordinates Add(CellCoordinates a, CellCoordinates b) =>
            new CellCoordinates(a.x + b.x, a.floor + b.floor);

        public static CellCoordinates Subtract(CellCoordinates a, CellCoordinates b) =>
            new CellCoordinates(a.x - b.x, a.floor - b.floor);

        public static bool Matches(CellCoordinates a, CellCoordinates b) =>
            (
                a.x == b.x &&
                a.floor == b.floor
            );

        public static CellCoordinates zero => new CellCoordinates(0, 0);
        public static CellCoordinates one => new CellCoordinates(1, 1);
    }
}


