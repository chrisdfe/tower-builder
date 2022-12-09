using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItemStack
    {
        public static FreightSizeMap<int> MaxFreightCountMap { get; } = new FreightSizeMap<int>(
            new Dictionary<FreightItem.Key, int>() {
                { FreightItem.Key.None,   0 },
                { FreightItem.Key.Small,  4 },
                { FreightItem.Key.Medium, 2 },
                { FreightItem.Key.Large,  1 }
            }
        );

        public List<FreightItem> freightItems { get; private set; } = new List<FreightItem>();

        public int count => freightItems.Count;

        public FreightItem first => freightItems[0];

        public FreightItem.Key size => first.key;

        public int maxCount
        {
            get => MaxFreightCountMap.FindBySize(first.key);
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