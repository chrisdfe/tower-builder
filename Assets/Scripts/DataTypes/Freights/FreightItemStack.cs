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

        public int count
        {
            get => freightItems.Count;
        }

        public FreightItem first
        {
            get => freightItems[0];
        }

        public FreightItem.Size size
        {
            get => first.size;
        }

        public int maxCount
        {
            get => MaxFreightCountMap.FindBySize(first.size);
        }

        public FreightItemStack(FreightItem freightItem)
        {
            freightItems.Add(freightItem);
        }

        public void Add(FreightItem freightItem)
        {
            if (count < maxCount)
            {
                freightItems.Add(freightItem);
            }
        }

        public void Remove(FreightItem freightItem)
        {
            freightItems.Remove(freightItem);
        }
    }
}