using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Windows;

namespace TowerBuilder.Definitions
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

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },
        };

        public WindowDefinitionsList() : base() { }
    }
}