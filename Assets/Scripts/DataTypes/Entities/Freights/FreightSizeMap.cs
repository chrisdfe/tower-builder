using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightSizeMap<T>
    {
        public Dictionary<FreightItem.Key, T> map { get; private set; } = new Dictionary<FreightItem.Key, T>();

        public T FindBySize(FreightItem.Key size) => map[size];

        public FreightSizeMap(Dictionary<FreightItem.Key, T> map)
        {
            this.map = map;
        }
    }
}