using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class WindowDefinitionsList : EntityDefinitionsList<Window.Key, WindowDefinition>
    {
        public override List<WindowDefinition> Definitions { get; } = new List<WindowDefinition>()
        {
            new WindowDefinition() {
                key = Window.Key.Default,
                title = "Basic",
                category = "Basic",

                resizability =  Resizability.Horizontal,

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1)
            },
        };

        public WindowDefinitionsList() : base() { }
    }
}