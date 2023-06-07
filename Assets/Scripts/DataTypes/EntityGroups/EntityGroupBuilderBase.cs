namespace TowerBuilder.DataTypes.EntityGroups
{
    public abstract class EntityGroupBuilderBase
    {
        public EntityGroupBuilderBase() { }

        public abstract EntityGroup Build(SelectionBox selectionBox);
    }
}


