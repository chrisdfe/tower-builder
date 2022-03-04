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
            float TILE_SIZE = Stores.Map.Constants.TILE_SIZE;

            return new Vector3(
                RoundToNearestTile(cellCoordinates.x),
                RoundToNearestTile(cellCoordinates.floor * TILE_SIZE),
                0
            );
        }

        public static bool CellIsOccupied(CellCoordinates cellCoordinates)
        {
            List<MapRoom> mapRooms = Registry.Stores.Map.mapRooms;

            foreach (MapRoom mapRoom in mapRooms)
            {
                foreach (CellCoordinates roomCellCoordiantes in mapRoom.roomCells.cells)
                {
                    if (cellCoordinates.Matches(roomCellCoordiantes))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}