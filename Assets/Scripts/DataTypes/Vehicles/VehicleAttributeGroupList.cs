using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class VehicleAttributeGroupList : ListWrapper<VehicleAttributeGroup>
    {
        public VehicleAttributeGroupList() : base() { }
        public VehicleAttributeGroupList(VehicleAttributeGroup vehicleAttributeGroup) : base(vehicleAttributeGroup) { }
        public VehicleAttributeGroupList(List<VehicleAttributeGroup> vehicleAttributeGroupList) : base(vehicleAttributeGroupList) { }
        public VehicleAttributeGroupList(VehicleAttributeGroupList vehicleAttributeGroupList) : base(vehicleAttributeGroupList) { }

        public VehicleAttributeGroup FindByVehicle(Vehicle vehicle)
        {
            return items.Find(vehicleAttributeGroup => vehicleAttributeGroup.vehicle == vehicle);
        }
    }
}