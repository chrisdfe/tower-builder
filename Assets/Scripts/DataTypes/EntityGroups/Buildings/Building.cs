using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.DataTypes.EntityGroups.Buildings
{
    public class Building : EntityGroup
    {
        public override string typeLabel => "Building";

        public Building() : base() { }
        public Building(BuildingDefinition definition) : base(definition) { }
    }
}