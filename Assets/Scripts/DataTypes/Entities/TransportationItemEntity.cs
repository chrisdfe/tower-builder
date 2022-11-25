using TowerBuilder.DataTypes.TransportationItems;

namespace TowerBuilder.DataTypes.Entities
{
    public class TransportationItemEntity : EntityBase
    {
        public TransportationItem transportationItem { get; private set; }

        public TransportationItemEntity(TransportationItem transportationItem)
        {
            this.transportationItem = transportationItem;
        }
    }
}