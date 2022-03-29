using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map.Rooms.EntranceBuilders
{
    public class StairwellEntranceBuilder : RoomEntranceBuilderBase
    {
        public override List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.GetWidth() - 1;
            int lowestX = roomCells.GetLowestX();
            int highestX = roomCells.GetHighestX();

            for (int floor = roomCells.GetLowestFloor(); floor <= roomCells.GetHighestFloor(); floor++)
            {
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = new CellCoordinates(lowestX, floor),
                    position = RoomEntrancePosition.Left
                });
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = new CellCoordinates(highestX, floor),
                    position = RoomEntrancePosition.Right
                });
            }

            return result;
        }
    }
}