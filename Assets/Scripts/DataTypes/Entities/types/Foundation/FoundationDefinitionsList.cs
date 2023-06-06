using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationDefinitionsList : EntityDefinitionsList<Foundation.Key, FoundationDefinition>
    {
        public override List<FoundationDefinition> Definitions { get; } = new List<FoundationDefinition>()
        {
            new FoundationDefinition() {
                key = Foundation.Key.Default,
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