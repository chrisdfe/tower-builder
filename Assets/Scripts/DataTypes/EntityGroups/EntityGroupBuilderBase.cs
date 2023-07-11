using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public abstract class EntityGroupBuilderBase
    {
        protected EntityGroupDefinition definition;

        public EntityGroupBuilderBase(EntityGroupDefinition definition)
        {
            this.definition = definition;
        }

        public abstract EntityGroup Build(AppState appState);
    }
}


