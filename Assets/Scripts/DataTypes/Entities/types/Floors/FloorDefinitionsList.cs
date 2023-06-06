using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.Definitions;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorDefinitionsList : EntityDefinitionsList<Floor.Key, FloorDefinition>
    {
        public override List<FloorDefinition> Definitions { get; } = new List<FloorDefinition>()
        {
            new FloorDefinition() {
                key = Floor.Key.Default,
                title = "Small",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            }
        };

        public FloorDefinitionsList() : base() { }
    }
}