using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItemStackGroup
    {
        public static FreightSizeMap<int> MaxStackCountMap { get; } = new FreightSizeMap<int>(
            new Dictionary<FreightItem.Key, int>() {
                { FreightItem.Key.None,   0 },
                { FreightItem.Key.Small,  4 },
                { FreightItem.Key.Medium, 2 },
                { FreightItem.Key.Large,  1 }
            }
        );

        public List<FreightItemStack> freightItemStacks { get; private set; } = new List<FreightItemStack>();
        public CellCoordinates cellCoordinates;
    }
}