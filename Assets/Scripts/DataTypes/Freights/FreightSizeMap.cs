using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Freights
{
    public class FreightSizeMap<T>
    {
        public Dictionary<FreightItem.Size, T> map { get; private set; } = new Dictionary<FreightItem.Size, T>();

        public T FindBySize(FreightItem.Size size) => map[size];

        public FreightSizeMap(Dictionary<FreightItem.Size, T> map)
        {
            this.map = map;
        }
    }
}