using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public static class GameWorldMapCellHelpers
    {
        public static int RoundToNearestTile(float number)
        {
            float TILE_SIZE = Stores.Rooms.Constants.TILE_SIZE;
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