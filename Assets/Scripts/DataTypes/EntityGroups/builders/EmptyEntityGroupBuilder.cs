namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EmptyEntityGroupBuilder : EntityGroupBuilderBase
    {
        public EmptyEntityGroupBuilder(EntityGroupDefinition definition) : base(definition) { }

        public override EntityGroup Build(SelectionBox selectionBox) => new EntityGroup();
    }
}


