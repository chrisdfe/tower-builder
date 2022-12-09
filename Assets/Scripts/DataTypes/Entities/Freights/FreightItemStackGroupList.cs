using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItemStackGroupList : ListWrapper<FreightItemStackGroup>
    {
        public FreightItemStackGroupList() { }
        public FreightItemStackGroupList(FreightItemStackGroup freightItemStackGroup) : base(freightItemStackGroup) { }
        public FreightItemStackGroupList(List<FreightItemStackGroup> freightItemStackGroupList) : base(freightItemStackGroupList) { }
        public FreightItemStackGroupList(FreightItemStackGroupList freightItemStackGroupList) : base(freightItemStackGroupList) { }
    }
}