namespace TowerBuilder.DataTypes.EntityGroups
{
    public abstract class EntityGroupBuilderBase<EntityGroupType>
        where EntityGroupType : EntityGroup, new()
    {
        public EntityGroupBuilderBase() { }

        public abstract EntityGroupType Build(SelectionBox selectionBox, bool isInBlueprintMode);
    }
}


