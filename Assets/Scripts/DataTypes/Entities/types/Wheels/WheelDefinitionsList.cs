using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class WheelDefinitionsList : EntityDefinitionsList<Wheel.Key, WheelDefinition>
    {
        public override List<WheelDefinition> Definitions { get; } = new List<WheelDefinition>()
        {
            new WheelDefinition() {
                key = Wheel.Key.Default,
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