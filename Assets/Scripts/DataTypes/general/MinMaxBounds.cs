using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class MinMaxBounds
    {
        public float min;
        public float max;

        public MinMaxBounds(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Clamp(float number)
        {
            if (number < min)
            {
                return min;
            }

            if (number > max)
            {
                return max;
            }

            return number;
        }
    }
}


