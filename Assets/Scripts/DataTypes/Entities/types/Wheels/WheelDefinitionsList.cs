using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class WheelDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new WheelDefinition() {
                key = "Default",
                title = "Small",
                category = "Basic",

                resizability = Resizability.Horizontal,

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },
        };

        public WheelDefinitionsList() : base() { }
    }
}