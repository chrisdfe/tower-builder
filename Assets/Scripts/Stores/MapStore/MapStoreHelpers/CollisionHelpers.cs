using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public static class MapStoreCollisionHelpers
    {
        public static bool coordinatesMatch(
          MapCoordinates coordinatesA,
          MapCoordinates coordinatesB
        )
        {
            return coordinatesA.x == coordinatesB.x && coordinatesA.y == coordinatesB.y;
        }

        public static bool cellIntersectsCoordinatesList(
          MapCoordinates targetCell,
          List<MapCoordinates> coordinatesList
        )
        {
            foreach (MapCoordinates coordinates in coordinatesList)
            {
                if (coordinatesMatch(targetCell, coordinates))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool cellIsOccupied(
          MapCoordinates targetCoordinates,
          MapCoordinates[] occupiedList
        )
        {
            foreach (MapCoordinates coordinates in occupiedList)
            {
                if (coordinatesMatch(coordinates, targetCoordinates))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool cellsAreOccupied(
          List<MapCoordinates> cellCoordinatesList,
          List<MapCoordinates> occupiedList
        )
        {
            foreach (MapCoordinates cellCoordinates in cellCoordinatesList)
            {
                if (cellIntersectsCoordinatesList(cellCoordinates, occupiedList))
                {
                    return true;
                }
            }

            return false;
        }
    }
}