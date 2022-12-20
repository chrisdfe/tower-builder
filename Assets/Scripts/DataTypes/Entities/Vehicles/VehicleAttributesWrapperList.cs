using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Entities.Rooms;

namespace TowerBuilder.DataTypes.Entities.Vehicles
{
    public class VehicleAttributesWrapperList : ListWrapper<VehicleAttributesWrapper>
    {
        public VehicleAttributesWrapperList() : base() { }
        public VehicleAttributesWrapperList(VehicleAttributesWrapper vehicleAttributesWrapper) : base(vehicleAttributesWrapper) { }
        public VehicleAttributesWrapperList(List<VehicleAttributesWrapper> vehicleAttributesWrapperList) : base(vehicleAttributesWrapperList) { }
        public VehicleAttributesWrapperList(VehicleAttributesWrapperList vehicleAttributesWrapperList) : base(vehicleAttributesWrapperList) { }

        public VehicleAttributesWrapper FindByVehicle(Vehicle vehicle) =>
            items.Find(vehicleAttributesWrapper => vehicleAttributesWrapper.vehicle == vehicle);
    }
}