using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class WindowDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new WindowDefinition() {
                key = "Default",
                title = "Basic",
                category = "Basic",

                resizability =  Resizability.Horizontal,

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1)
            },
        };

        public WindowDefinitionsList() : base() { }
    }
}