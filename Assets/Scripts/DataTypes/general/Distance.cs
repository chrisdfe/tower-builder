using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Distance
    {
        public int x;
        public int floors;

        public Distance(int x, int floors)
        {
            this.x = x;
            this.floors = floors;
        }

        public override string ToString() => $"x: {x}, floors: {floors}";

        public bool Matches(Distance otherDistance) =>
            x == otherDistance.x && floors == otherDistance.floors;

        /*
            Static API
        */
        public static Distance one
        {
            get => new Distance(1, 1);
        }
    }
}


