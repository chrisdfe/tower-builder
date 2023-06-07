using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new TransportationItemDefinition() {
                key = "Escalator",
                title = "Escalator",
                category = "Escalators",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(1, 1)
                    }
                ),
                staticBlockSize = false,

                entranceExitBuilder = (TransportationItem transporationItem) => {
                    List<(CellCoordinates, CellCoordinates)> result = new List<(CellCoordinates, CellCoordinates)>();

                    // bottom to top
                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.cellCoordinatesList.topRightCoordinates
                        )
                    );

                    // top to bottom
                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.topRightCoordinates,
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates
                        )
                    );

                    // TODO - both ways if the transportation item is two way

                    return result;
                },

                pricePerCell = 800,

                resizability = Resizability.Diagonal
            },

            new TransportationItemDefinition()
            {
                key = "Ladder",
                title = "Ladder",
                category = "Ladders",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(0, 1)
                    }
                ),

                entranceExitBuilder = (TransportationItem transporationItem) => {
                    List<(CellCoordinates, CellCoordinates)> result = new List<(CellCoordinates, CellCoordinates)>();

                    // bottom to top
                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.cellCoordinatesList.topLeftCoordinates
                        )
                    );

                    // top to bottom
                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.topLeftCoordinates,
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates
                        )
                    );

                    return result;
                },
                staticBlockSize = false,

                pricePerCell = 400,

                resizability = Resizability.Vertical
            },

            new TransportationItemDefinition()
            {
                key = "Doorway",
                title = "Doorway",
                category = "Doorways",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(1, 0)
                    }
                ),

                entranceExitBuilder = (TransportationItem transporationItem) => {
                    List<(CellCoordinates, CellCoordinates)> result = new List<(CellCoordinates, CellCoordinates)>();

                    // left to right
                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.cellCoordinatesList.bottomRightCoordinates
                        )
                    );

                    // right to left
                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomRightCoordinates,
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates
                        )
                    );

                    return result;
                },

                pricePerCell = 200,

                resizability = Resizability.Inflexible
            }
        };

        public TransportationItemDefinitionsList() : base() { }
    }
}