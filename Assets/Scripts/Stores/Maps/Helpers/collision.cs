using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map
{
    //
    // Helpers that check for collision between cells and groups of cells
    //
    public partial class MapStore
    {
        public static partial class Helpers
        {
            public static bool cellCoordinatesMatch(
              CellCoordinates coordinatesA,
              CellCoordinates coordinatesB
            )
            {
                return (
                    coordinatesA.x == coordinatesB.x &&
                    // coordinatesA.z == coordinatesB.z &&
                    coordinatesA.floor == coordinatesB.floor
                );
            }

            public static bool cellIntersectsCoordinatesList(
              CellCoordinates targetCell,
              List<CellCoordinates> coordinatesList
            )
            {
                foreach (CellCoordinates coordinates in coordinatesList)
                {
                    if (cellCoordinatesMatch(targetCell, coordinates))
                    {
                        return true;
                    }
                }

                return false;
            }

            // public static bool cellsAreOccupied(
            //   List<MapCoordinates> cellCoordinatesList,
            //   List<MapCoordinates> occupiedList
            // )
            // {
            //     foreach (MapCoordinates cellCoordinates in cellCoordinatesList)
            //     {
            //         if (cellIntersectsCoordinatesList(cellCoordinates, occupiedList))
            //         {
            //             return true;
            //         }
            //     }

            //     return false;
            // }
        }
    }
}