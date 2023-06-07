using System.Collections.Generic;
using TowerBuilder.Definitions;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions =>
            new List<EntityDefinition>()
                {
                    new FreightDefinition() {
                        key = "Small",
                        title = "Small",
                        category = "Basic",

                        blockCellsTemplate = new CellCoordinatesList(
                            new List<CellCoordinates>() {
                                new CellCoordinates(0, 0),
                            }
                        ),
                    },

                    new FreightDefinition() {
                        key = "Medium",
                        title = "Medium",
                        category = "Basic",

                        blockCellsTemplate = CellCoordinatesList.CreateRectangle(2, 2)
                    },

                    new FreightDefinition() {
                        key = "Large",
                        title = "Large",
                        category = "Basic",

                        blockCellsTemplate = CellCoordinatesList.CreateRectangle(4, 3)
                    },
                };

        public FreightDefinitionsList() : base() { }
    }
}