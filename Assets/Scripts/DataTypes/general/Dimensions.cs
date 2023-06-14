using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Dimensions
    {
        public int width;
        public int height;

        public Dimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override string ToString() => $"width: {width}, height: {height}";

        public bool Matches(Dimensions otherDimensions) =>
            width == otherDimensions.width && height == otherDimensions.height;

        /*
            Static Interface
        */
        public static Dimensions one
        {
            get => new Dimensions(1, 1);
        }
    }
}


