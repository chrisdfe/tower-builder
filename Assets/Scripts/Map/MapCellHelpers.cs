using System;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.UI
{
    public static class MapCellHelpers
    {
        public static int RoundToNearestTile(float number)
        {
            float TILE_SIZE = Stores.Map.Constants.TILE_SIZE;
            float rounded = (float)Math.Round(number / TILE_SIZE) * TILE_SIZE;
            return (int)rounded;
        }

        public static Vector3 CellCoordinatesToPosition(CellCoordinates cellCoordinates)
        {
            float TILE_SIZE = Stores.Map.Constants.TILE_SIZE;

            return new Vector3(
                cellCoordinates.x * TILE_SIZE,
                cellCoordinates.floor * TILE_SIZE,
                cellCoordinates.z * TILE_SIZE
            );
        }
    }
}