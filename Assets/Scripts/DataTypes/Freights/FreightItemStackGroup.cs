using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Freights
{
    public class FreightItemStackGroup
    {
        public static FreightSizeMap<int> MaxStackCountMap { get; } = new FreightSizeMap<int>(
            new Dictionary<FreightItem.Size, int>() {
                { FreightItem.Size.None,   0 },
                { FreightItem.Size.Small,  4 },
                { FreightItem.Size.Medium, 2 },
                { FreightItem.Size.Large,  1 }
            }
        );

        public List<FreightItemStack> freightItemStacks { get; private set; } = new List<FreightItemStack>();
        public CellCoordinates cellCoordinates;
    }
}