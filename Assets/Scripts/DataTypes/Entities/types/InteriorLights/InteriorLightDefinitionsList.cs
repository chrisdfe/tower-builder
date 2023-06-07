using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.InteriorLights
{
    public class InteriorLightDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new InteriorLightDefinition() {
                key = "Default",
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