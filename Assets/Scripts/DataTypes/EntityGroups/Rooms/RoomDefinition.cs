using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomDefinition : EntityGroupDefinition
    {
        public override ValidatorFactory buildValidatorFactory => (EntityGroup entityGroup) => new RoomBuildValidator(entityGroup as Room);

        public override BuilderFactory builderFactory { get; set; } =
            (EntityGroupDefinition definition) => new RoomBuilderBase(definition);
    }
}

