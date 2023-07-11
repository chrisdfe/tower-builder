namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupDefinition
    {
        public virtual string key { get; set; } = "None";
        public virtual string title { get; set; } = "None";
        public virtual string category { get; set; } = "None";

        public delegate EntityGroupValidator ValidatorFactory(EntityGroup entityGroup);

        public virtual ValidatorFactory buildValidatorFactory { get; set; } =
            (EntityGroup entityGroup) => new EntityGroupValidator(entityGroup);

        public virtual ValidatorFactory destroyValidatorFactory { get; set; } =
            (EntityGroup entityGroup) => new EntityGroupValidator(entityGroup);

        public delegate EntityGroupBuilderBase BuilderFactory(EntityGroupDefinition definition);
        public virtual BuilderFactory builderFactory { get; set; } =
            (EntityGroupDefinition definition) => new EmptyEntityGroupBuilder(definition);
    }
}