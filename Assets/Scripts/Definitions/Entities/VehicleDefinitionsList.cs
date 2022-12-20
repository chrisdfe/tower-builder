using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class VehicleDefinitionsList : EntityDefinitionsList<Vehicle.Key, VehicleDefinition>
    {
        public override List<VehicleDefinition> Definitions { get; } = new List<VehicleDefinition>() {
            new VehicleDefinition()
            {
                title = "Default",
                key = Vehicle.Key.Default,
                category = "Default",
            }
        };

        public VehicleDefinitionsList() : base() { }
    }
}
