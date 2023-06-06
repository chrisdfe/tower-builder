using System.Collections.Generic;
using TowerBuilder.Definitions;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightDefinitionsList : EntityDefinitionsList<FreightItem.Key, FreightDefinition>
    {
        public override List<FreightDefinition> Definitions =>
            new List<FreightDefinition>()
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

                        blockCellsTemplate = CellCoordinatesList.CreateRectangle(2, 2)
                    },

                    new FreightDefinition() {
                        key = FreightItem.Key.Large,
                        title = "Large",
                        category = "Basic",

                        blockCellsTemplate = CellCoordinatesList.CreateRectangle(4, 3)
                    },
                };

        public FreightDefinitionsList() : base() { }
    }
}