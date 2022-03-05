using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
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
            return new Vector3(
                RoundToNearestTile(cellCoordinates.x),
                RoundToNearestTile(cellCoordinates.floor),
                0
            );
        }
    }
}