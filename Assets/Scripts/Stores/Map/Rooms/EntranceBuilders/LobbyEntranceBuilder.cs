using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map.Rooms.EntranceBuilders
{
    public static class LobbyEntranceBuilder
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