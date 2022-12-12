using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Freights;

namespace TowerBuilder.Definitions
{
    public class FreightDefinitionsList : EntityDefinitionsList<FreightItem.Key, FreightItem, FreightDefinition>
    {
        public override List<FreightDefinition> Definitions { get; } = new List<FreightDefinition>()
        {
            new FreightDefinition() {
                key = FreightItem.Key.Small,
                title = "Small",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },

            new FreightDefinition() {
                key = FreightItem.Key.Medium,
                title = "Medium",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },

            new FreightDefinition() {
                key = FreightItem.Key.Large,
                title = "Large",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },
        };

        public FreightDefinitionsList() : base() { }
    }
}