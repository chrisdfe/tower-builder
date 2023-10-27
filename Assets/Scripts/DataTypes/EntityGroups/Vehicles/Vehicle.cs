using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.DataTypes.EntityGroups.Vehicles
{
    public class Vehicle : EntityGroup
    {
        public override string typeLabel => "Vehicle";

        public VehicleAttributes attributes;

        public Vehicle() : base()
        {
            attributes = new VehicleAttributes(this);
        }

        public Vehicle(VehicleDefinition definition) : base(definition)
        {
            attributes = new VehicleAttributes(this);
        }
    }
}