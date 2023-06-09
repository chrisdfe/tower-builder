using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class OfficeRoomDefinition : RoomDefinition
    {
        public override BuilderFactory builderFactory => (EntityGroupDefinition definition) => new OfficeRoomBuilder(definition);
    }
}

