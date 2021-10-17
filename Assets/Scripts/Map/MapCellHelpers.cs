using System;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.UI
{
    public static class MapCellHelpers
    {
        public static int RoundToNearestTile(float number)
        {
            float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;
            float rounded = (float)Math.Round(number / TILE_SIZE) * TILE_SIZE;
            return (int)rounded;
        }

        // public static CellCoordinates WorldPositionToCellCoordinates(Vector3 point)
        // {
        //     int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
        //     float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;

        //     return new Vector3(
        //         RoundToNearestTile(point.x),
        //         // (currentFocusFloor * TILE_SIZE) + (TILE_SIZE / 2),
        //         RoundToNearestTile(point.y),
        //         RoundToNearestTile(point.z)
        //     );
        // }

        public static Vector3 CellCoordinatesToPosition(CellCoordinates cellCoordinates)
        {

            float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;
            return new Vector3(
                cellCoordinates.x * TILE_SIZE,
                cellCoordinates.floor * TILE_SIZE,
                cellCoordinates.z * TILE_SIZE
            );
        }
    }
}