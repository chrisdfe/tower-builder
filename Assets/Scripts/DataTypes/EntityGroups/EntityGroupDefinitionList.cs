namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupDefinitionList
    {
        public virtual ListWrapper<EntityGroupDefinition> Definitions { get; }

        public EntityGroupDefinition FindByKey(string key) =>
            Definitions.Find(definition => definition.key == key);
    }
}