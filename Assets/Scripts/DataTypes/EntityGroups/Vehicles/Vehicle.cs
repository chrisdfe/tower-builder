using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.DataTypes.EntityGroups.Vehicles
{
    public class Vehicle : EntityGroup
    {
        // public override string idKey => "vehicles";

        public ListWrapper<Room> roomList { get; private set; } = new ListWrapper<Room>();

        // public Vehicle(VehicleDefinition vehicleDefinition) : base() { }
        public Vehicle() : base() { }
    }
}