using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.Definitions;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new FloorDefinition() {
                key = "Default",
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