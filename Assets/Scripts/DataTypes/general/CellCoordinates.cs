using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CellCoordinates
    {
        [JsonProperty]
        public int x;

        [JsonProperty]
        public int y;

        public CellCoordinates coordinatesAbove => new CellCoordinates(x, y + 1);

        public CellCoordinates coordinatesAboveRight => new CellCoordinates(x + 1, y + 1);

        public CellCoordinates coordinatesRight => new CellCoordinates(x + 1, y);

        public CellCoordinates coordinatesBelowRight => new CellCoordinates(x + 1, y - 1);

        public CellCoordinates coordinatesBelow => new CellCoordinates(x, y - 1);

        public CellCoordinates coordinatesBelowLeft => new CellCoordinates(x - 1, y - 1);

        public CellCoordinates coordinatesLeft => new CellCoordinates(x - 1, y);

        public CellCoordinates coordinatesAboveLeft => new CellCoordinates(x - 1, y + 1);

        public CellCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"x: {x}, y: {y}";

        // TODO - this should mutate
        public CellCoordinates Add(CellCoordinates b) => CellCoordinates.Add(this, b);

        // TODO - this should mutate
        public CellCoordinates Subtract(CellCoordinates b) => CellCoordinates.Subtract(this, b);

        public bool Matches(CellCoordinates b) => CellCoordinates.Matches(this, b);

        public CellCoordinates Clone() => new CellCoordinates(x, y);

        /* 
            Static Interface
        */
        public static CellCoordinates Add(CellCoordinates a, CellCoordinates b) =>
            new CellCoordinates(a.x + b.x, a.y + b.y);

        public static CellCoordinates Subtract(CellCoordinates a, CellCoordinates b) =>
            new CellCoordinates(a.x - b.x, a.y - b.y);

        public static bool Matches(CellCoordinates a, CellCoordinates b) =>
            (
                a.x == b.x &&
                a.y == b.y
            );

        public static CellCoordinates zero => new CellCoordinates(0, 0);
        public static CellCoordinates one => new CellCoordinates(1, 1);
    }
}


