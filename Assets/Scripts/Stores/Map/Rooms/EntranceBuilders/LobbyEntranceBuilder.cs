using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.Map.Rooms.Uses;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.EntranceBuilders
{
    public static class LobbyEntranceBuilder
    {
        public static List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.GetWidth() - 1;
            int floor = roomCells.GetLowestFloor();

            result.Add(new RoomEntrance()
            {
                cellCoordinates = new CellCoordinates(0, floor),
                position = RoomEntrancePosition.Left
            });
            result.Add(new RoomEntrance()
            {
                cellCoordinates = new CellCoordinates(width, floor),
                position = RoomEntrancePosition.Right
            });

            return result;
        }
    }
}