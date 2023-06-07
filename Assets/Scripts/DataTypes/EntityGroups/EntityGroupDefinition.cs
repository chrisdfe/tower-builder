namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupDefinition<EntityGroupKey>
        where EntityGroupKey : struct
    {
        public virtual string title { get; set; } = "None";
        public virtual string category { get; set; } = "None";

        public virtual Resizability resizability { get; set; } = Resizability.Flexible;

        public delegate EntityGroupValidator ValidatorFactory(EntityGroup entityGroup);
        public virtual ValidatorFactory validatorFactory => (EntityGroup entityGroup) => new EntityGroupValidator(entityGroup);
    }
}