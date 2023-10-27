using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class Floor : Entity
    {
        public override string idKey { get => "floors"; }

        public Room room;

        public Floor(FloorDefinition floorDefinition) : base(floorDefinition) { }
    }
}