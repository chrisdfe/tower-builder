using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class TransportationItemDefinitionsList : EntityDefinitionsList<TransportationItem.Key, TransportationItemDefinition>
    {
        public override List<TransportationItemDefinition> Definitions { get; } = new List<TransportationItemDefinition>()
        {
            new TransportationItemDefinition() {
                key = TransportationItem.Key.Escalator,
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

                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.cellCoordinatesList.topRightCoordinates
                        )
                    );

                    // TODO - both ways if the transportation item is two way

                    return result;
                },

                pricePerCell = 800,

                resizability = TransportationItem.Resizability.Diagonal
                // resizability = TransportationItem.Resizability.Flexible
            },

            new TransportationItemDefinition()
        {
            key = TransportationItem.Key.Ladder,
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

                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.cellCoordinatesList.topLeftCoordinates
                        )
                    );

                    return result;
                },
                staticBlockSize = false,

                pricePerCell = 400,

                resizability = TransportationItem.Resizability.Vertical
            },

            new TransportationItemDefinition()
        {
            key = TransportationItem.Key.Doorway,
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

                    result.Add(
                        (
                            transporationItem.cellCoordinatesList.bottomLeftCoordinates,
                            transporationItem.cellCoordinatesList.bottomRightCoordinates
                        )
                    );

                    return result;
                },

                pricePerCell = 200,

                resizability = TransportationItem.Resizability.Inflexible
            }
    };

        public TransportationItemDefinitionsList() : base() { }
    }
}