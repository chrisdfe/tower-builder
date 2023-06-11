namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupDefinition
    {
        public virtual string key { get; set; } = "None";
        public virtual string title { get; set; } = "None";
        public virtual string category { get; set; } = "None";

        // public virtual Resizability resizability { get; set; } = Resizability.Flexible;

        public delegate EntityGroupValidator ValidatorFactory(EntityGroup entityGroup);
        public virtual ValidatorFactory validatorFactory { get; set; } =
            (EntityGroup entityGroup) => new EntityGroupValidator(entityGroup);

        public delegate EntityGroupBuilderBase BuilderFactory(EntityGroupDefinition definition);
        public virtual BuilderFactory builderFactory { get; set; } =
            (EntityGroupDefinition definition) => new EmptyEntityGroupBuilder(definition);
    }
}