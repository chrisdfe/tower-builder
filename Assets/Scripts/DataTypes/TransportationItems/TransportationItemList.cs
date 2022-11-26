using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.TransportationItems
{
    public class TransportationItemsList : ListWrapper<TransportationItem>
    {
        public TransportationItemsList() { }
        public TransportationItemsList(TransportationItem transportationItem) : base(transportationItem) { }
        public TransportationItemsList(List<TransportationItem> transportationItems) : base(transportationItems) { }
        public TransportationItemsList(ListWrapper<TransportationItem> transportationItemList) : base(transportationItemList) { }
    }
}