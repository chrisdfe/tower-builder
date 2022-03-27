using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.Map.Rooms.Uses;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.EntranceBuilders
{
    public static class HallwayEntranceBuilder
    {
        public static List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.GetWidth();

            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.GetBottomLeftCoordinates(),
                position = RoomEntrancePosition.Left
            });


            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.GetBottomRightCoordinates(),
                position = RoomEntrancePosition.Right
            });


            return result;
        }
    }
}