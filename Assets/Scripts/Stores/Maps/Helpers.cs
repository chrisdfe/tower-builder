using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static partial class Helpers
        {
            // public static bool CellCoordinatesMatch(
            //   CellCoordinates coordinatesA,
            //   CellCoordinates coordinatesB
            // )
            // {
            //     return (
            //         coordinatesA.x == coordinatesB.x &&
            //         // coordinatesA.z == coordinatesB.z &&
            //         coordinatesA.floor == coordinatesB.floor
            //     );
            // }

            // public static bool CellIntersectsCoordinatesList(
            //   CellCoordinates targetCell,
            //   List<CellCoordinates> coordinatesList
            // )
            // {
            //     foreach (CellCoordinates coordinates in coordinatesList)
            //     {
            //         if (CellCoordinatesMatch(targetCell, coordinates))
            //         {
            //             return true;
            //         }
            //     }

            //     return false;
            // }

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

            // public static List<CellCoordinates> CreateRectangularRoomBlueprint(int xSize, int floors)
            // {
            //     List<CellCoordinates> result = new List<CellCoordinates>();

            //     for (int x = 0; x < xSize; x++)
            //     {
            //         for (int floor = 0; floor < floors; floor++)
            //         {
            //             result.Add(new CellCoordinates()
            //             {
            //                 x = x,
            //                 // z = z,
            //                 floor = floor
            //             });
            //         }
            //     }

            //     return result;
            // }
        }
    }
}