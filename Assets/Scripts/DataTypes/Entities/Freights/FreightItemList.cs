using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItemList : ListWrapper<FreightItem>
    {
        public FreightItemList() { }
        public FreightItemList(FreightItem freight) : base(freight) { }
        public FreightItemList(List<FreightItem> freightItemList) : base(freightItemList) { }
        public FreightItemList(FreightItemList freightList) : base(freightList) { }
    }
}