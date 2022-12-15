using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.InteriorWalls;

namespace TowerBuilder.Definitions
{
    public class InteriorWallDefinitionsList : EntityDefinitionsList<InteriorWall.Key, InteriorWallDefinition>
    {
        public override List<InteriorWallDefinition> Definitions { get; } = new List<InteriorWallDefinition>()
        {
            new InteriorWallDefinition() {
                key = InteriorWall.Key.Default,
                title = "Left",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            },

            new InteriorWallDefinition() {
                key = InteriorWall.Key.Default,
                title = "Right",
                category = "Basic",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),
            }
        };

        public InteriorWallDefinitionsList() : base() { }
    }
}