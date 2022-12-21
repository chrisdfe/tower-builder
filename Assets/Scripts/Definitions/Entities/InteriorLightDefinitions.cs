using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.InteriorLights;

namespace TowerBuilder.Definitions
{
    public class InteriorLightDefinitionsList : EntityDefinitionsList<InteriorLight.Key, InteriorLightDefinition>
    {
        public override List<InteriorLightDefinition> Definitions { get; } = new List<InteriorLightDefinition>()
        {
            new InteriorLightDefinition() {
                key = InteriorLight.Key.Default,
                title = "Top",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },
        };

        public InteriorLightDefinitionsList() : base() { }
    }
}