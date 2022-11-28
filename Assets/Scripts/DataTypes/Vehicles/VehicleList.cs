using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class VehicleList : ListWrapper<Vehicle>
    {
        public VehicleList() : base() { }
        public VehicleList(Vehicle vehicle) : base(vehicle) { }
        public VehicleList(List<Vehicle> vehicles) : base(vehicles) { }
        public VehicleList(VehicleList vehicleList) : base(vehicleList) { }
    }
}