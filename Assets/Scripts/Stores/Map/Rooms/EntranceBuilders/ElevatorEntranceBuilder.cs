using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.Map.Rooms.Uses;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.EntranceBuilders
{
    public static class ElevatorEntranceBuilder
    {
        public static List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.GetWidth() - 1;

            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.GetBottomLeftCoordinates().Clone(),
                position = RoomEntrancePosition.Left
            });
            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.GetBottomRightCoordinates().Clone(),
                position = RoomEntrancePosition.Right
            });

            int floorSpan = roomCells.GetFloorSpan();
            if (floorSpan > 1)
            {
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = roomCells.GetTopLeftCoordinates(),
                    position = RoomEntrancePosition.Left
                });
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = roomCells.GetTopRightCoordinates(),
                    position = RoomEntrancePosition.Right
                });
            }

            return result;
        }
    }
}