using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class VehicleAttributesWrapperList : ListWrapper<VehicleAttributesWrapper>
    {
        public VehicleAttributesWrapperList() : base() { }
        public VehicleAttributesWrapperList(VehicleAttributesWrapper vehicleAttributesWrapper) : base(vehicleAttributesWrapper) { }
        public VehicleAttributesWrapperList(List<VehicleAttributesWrapper> vehicleAttributesWrapperList) : base(vehicleAttributesWrapperList) { }
        public VehicleAttributesWrapperList(VehicleAttributesWrapperList vehicleAttributesWrapperList) : base(vehicleAttributesWrapperList) { }

        public VehicleAttributesWrapper FindByVehicle(Vehicle vehicle)
        {
            return items.Find(vehicleAttributesWrapper => vehicleAttributesWrapper.vehicle == vehicle);
        }
    }
}