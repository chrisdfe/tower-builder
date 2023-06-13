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
                            transporationItem.relativeCellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.relativeCellCoordinatesList.topRightCoordinates
                        )
                    );

                    // top to bottom
                    result.Add(
                        (
                            transporationItem.relativeCellCoordinatesList.topRightCoordinates,
                            transporationItem.relativeCellCoordinatesList.bottomLeftCoordinates
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
                            transporationItem.relativeCellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.relativeCellCoordinatesList.topLeftCoordinates
                        )
                    );

                    // top to bottom
                    result.Add(
                        (
                            transporationItem.relativeCellCoordinatesList.topLeftCoordinates,
                            transporationItem.relativeCellCoordinatesList.bottomLeftCoordinates
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
                            transporationItem.relativeCellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.relativeCellCoordinatesList.bottomRightCoordinates
                        )
                    );

                    // right to left
                    result.Add(
                        (
                            transporationItem.relativeCellCoordinatesList.bottomRightCoordinates,
                            transporationItem.relativeCellCoordinatesList.bottomLeftCoordinates
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