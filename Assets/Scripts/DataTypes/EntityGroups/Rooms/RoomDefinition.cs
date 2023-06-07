namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomDefinition : EntityGroupDefinition<Room.Key>
    {
        public override ValidatorFactory validatorFactory => (EntityGroup entityGroup) => new RoomValidator(entityGroup as Room);

        public delegate RoomBuilderBase RoomBuilderFactory();
        public virtual RoomBuilderFactory roomBuilderFactory => () => new RoomBuilderBase();
    }
}

