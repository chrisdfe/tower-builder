using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Freights
{
    public class FreightItem
    {
        public enum Size
        {
            None,
            Small,
            Medium,
            Large
        }

        public Size size;
    }
}