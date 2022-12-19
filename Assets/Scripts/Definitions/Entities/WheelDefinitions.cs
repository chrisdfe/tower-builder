using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.Definitions
{
    public class WheelDefinitionsList : EntityDefinitionsList<Wheel.Key, WheelDefinition>
    {
        public override List<WheelDefinition> Definitions { get; } = new List<WheelDefinition>()
        {
            new WheelDefinition() {
                key = Wheel.Key.Default,
                title = "Small",
                category = "Basic",

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