namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomDefinition : EntityGroupDefinition<Room.Key>
    {
        public override ValidatorFactory validatorFactory => (EntityGroup entityGroup) => new RoomValidator(entityGroup as Room);

        public delegate RoomBuilderBase RoomBuilderFactory(EntityGroupDefinition definition);
        public virtual RoomBuilderFactory roomBuilderFactory => (EntityGroupDefinition definition) => new RoomBuilderBase(definition);
    }
}

