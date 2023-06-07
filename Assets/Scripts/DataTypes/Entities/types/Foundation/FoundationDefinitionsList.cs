using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new FoundationDefinition() {
                key = "Default",
                title = "Default",
                category = "Default",

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1),

                staticBlockSize = false,

                pricePerCell = 800,

                resizability = Resizability.Flexible
            }
        };

        public FoundationDefinitionsList() : base() { }
    }
}