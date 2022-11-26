using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public static class GameWorldMapCellHelpers
    {
        public static int RoundToNearestTile(float number)
        {
            float TILE_SIZE = DataTypes.Rooms.Constants.TILE_SIZE;
            float rounded = (float)Math.Round(number / TILE_SIZE) * TILE_SIZE;
            return (int)rounded;
        }

        /* 
            Static API
        */
        public static Vector3 CellCoordinatesToPosition(CellCoordinates cellCoordinates, float zIndex = 0f)
        {
            return new Vector3(
                RoundToNearestTile(cellCoordinates.x),
                RoundToNearestTile(cellCoordinates.floor),
                zIndex
            );
        }
    }
}