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

    public class FreightItemList : ListWrapper<FreightItem>
    {
        public FreightItemList() { }
        public FreightItemList(FreightItem freight) : base(freight) { }
        public FreightItemList(List<FreightItem> freightItemList) : base(freightItemList) { }
        public FreightItemList(FreightItemList freightList) : base(freightList) { }
    }
}