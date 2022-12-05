using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Freights
{
    public class FreightItemStackList : ListWrapper<FreightItemStack>
    {
        public FreightItemStackList() { }
        public FreightItemStackList(FreightItemStack freightItemStack) : base(freightItemStack) { }
        public FreightItemStackList(List<FreightItemStack> freightItemStackList) : base(freightItemStackList) { }
        public FreightItemStackList(FreightItemStackList freightItemStackList) : base(freightItemStackList) { }
    }
}