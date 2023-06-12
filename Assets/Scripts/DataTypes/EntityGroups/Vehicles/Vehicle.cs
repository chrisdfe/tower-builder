using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.DataTypes.EntityGroups.Vehicles
{
    public class Vehicle : EntityGroup
    {
        public override string typeLabel => "Vehicle";

        public Vehicle() : base() { }
        public Vehicle(VehicleDefinition definition) : base(definition) { }
    }
}