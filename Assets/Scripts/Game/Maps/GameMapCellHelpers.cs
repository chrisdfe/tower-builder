using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public static class GameMapCellHelpers
    {
        public static int RoundToNearestTile(float number)
        {
            float TILE_SIZE = Stores.Map.Rooms.Constants.TILE_SIZE;
            float rounded = (float)Math.Floor(number / TILE_SIZE) * TILE_SIZE;
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