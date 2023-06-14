using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Distance
    {
        public int x;
        public int y;

        public Distance(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"x: {x}, y: {y}";

        public bool Matches(Distance otherDistance) =>
            x == otherDistance.x && y == otherDistance.y;

        /*
            Static Interface
        */
        public static Distance one
        {
            get => new Distance(1, 1);
        }
    }
}


