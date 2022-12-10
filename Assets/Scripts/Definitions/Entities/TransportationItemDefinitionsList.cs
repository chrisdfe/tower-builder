using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class TransportationItemDefinitionsList : EntityDefinitionsList<TransportationItem.Key, TransportationItem, TransportationItemDefinition>
    {
        public override List<TransportationItemDefinition> Definitions { get; } = new List<TransportationItemDefinition>()
        {
            new TransportationItemDefinition() {
                key = TransportationItem.Key.Escalator,
                title = "Escalator",
                category = "Escalators",
                cellCoordinatesList = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(1, 1)
                    }
                ),
                entranceCellCoordinates = new CellCoordinates(0, 0),
                exitCellCoordinates = new CellCoordinates(1, 1),
                pricePerCell = 800,

                resizability = TransportationItem.Resizability.Diagonal
                // resizability = TransportationItem.Resizability.Flexible
            },

            new TransportationItemDefinition() {
                key = TransportationItem.Key.Ladder,
                title = "Ladder",
                category = "Ladders",
                cellCoordinatesList = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(0, 1)
                    }
                ),
                entranceCellCoordinates = new CellCoordinates(0, 0),
                exitCellCoordinates = new CellCoordinates(0, 1),
                pricePerCell = 400,

                resizability = TransportationItem.Resizability.Vertical
            },

            new TransportationItemDefinition() {
                key = TransportationItem.Key.Doorway,
                title = "Doorway",
                category = "Doorways",
                cellCoordinatesList = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(1, 0)
                    }
                ),
                entranceCellCoordinates = new CellCoordinates(0, 0),
                exitCellCoordinates = new CellCoordinates(1, 0),
                pricePerCell = 200,

                resizability = TransportationItem.Resizability.Inflexible
            }
        };

        public TransportationItemDefinitionsList() : base() { }
    }
}