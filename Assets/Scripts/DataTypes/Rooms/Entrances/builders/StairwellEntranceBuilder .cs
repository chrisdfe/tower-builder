using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Rooms.Entrances
{
    public class StairwellEntranceBuilder : RoomEntranceBuilderBase
    {
        public override List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.coordinatesList.GetWidth() - 1;
            int lowestX = roomCells.coordinatesList.GetLowestX();
            int highestX = roomCells.coordinatesList.GetHighestX();

            for (int floor = roomCells.coordinatesList.GetLowestFloor(); floor <= roomCells.coordinatesList.GetHighestFloor(); floor++)
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