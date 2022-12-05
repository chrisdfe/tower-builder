using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Freights
{
    public class FreightItemStack
    {
        public static FreightSizeMap<int> MaxFreightCountMap { get; } = new FreightSizeMap<int>(
            new Dictionary<FreightItem.Size, int>() {
                { FreightItem.Size.None,   0 },
                { FreightItem.Size.Small,  4 },
                { FreightItem.Size.Medium, 2 },
                { FreightItem.Size.Large,  1 },
            }
        );

        public List<FreightItem> freightItems { get; private set; } = new List<FreightItem>();

        public FreightItem.Size size
        {
            get => freightItems[0].size;
        }

        public FreightItemStack(FreightItem freight)
        {
            freightItems.Add(freight);
        }
    }
}