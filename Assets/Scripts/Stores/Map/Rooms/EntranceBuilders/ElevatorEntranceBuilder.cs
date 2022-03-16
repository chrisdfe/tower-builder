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
                cellCoordinates = new CellCoordinates(0, 0),
                position = RoomEntrancePosition.Left
            });
            result.Add(new RoomEntrance()
            {
                cellCoordinates = new CellCoordinates(width, 0),
                position = RoomEntrancePosition.Right
            });

            int floorSpan = roomCells.GetFloorSpan();
            if (floorSpan > 1)
            {
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = new CellCoordinates(0, -floorSpan),
                    position = RoomEntrancePosition.Left
                });
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = new CellCoordinates(width, -floorSpan),
                    position = RoomEntrancePosition.Right
                });
            }

            // Debug.Log("Here is the result:");
            // foreach (RoomEntrance entrance in result)
            // {
            //     Debug.Log(entrance.cellCoordinates);
            // }

            Debug.Log(roomCells.GetTopLeftCoordinates());

            return result;
        }
    }
}