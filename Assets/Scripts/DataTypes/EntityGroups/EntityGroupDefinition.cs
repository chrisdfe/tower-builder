namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupDefinition
    {
        public virtual string title { get; set; } = "None";
        public virtual string category { get; set; } = "None";

        // public virtual Resizability resizability { get; set; } = Resizability.Flexible;

        public delegate EntityGroupValidator ValidatorFactory(EntityGroup entityGroup);
        public virtual ValidatorFactory validatorFactory => (EntityGroup entityGroup) => new EntityGroupValidator(entityGroup);

        public delegate EntityGroupBuilderBase BuilderFactory(EntityGroupDefinition definition);
        public BuilderFactory builderFactory => (EntityGroupDefinition definition) => new EmptyEntityGroupBuilder(definition);
    }

    public class EntityGroupDefinition<KeyType> : EntityGroupDefinition
        where KeyType : struct
    {
        public KeyType key;
    }
}