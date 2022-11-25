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
            int width = roomCells.coordinatesList.width - 1;
            int lowestX = roomCells.coordinatesList.lowestX;
            int highestX = roomCells.coordinatesList.highestX;

            for (int floor = roomCells.coordinatesList.lowestFloor; floor <= roomCells.coordinatesList.highestFloor; floor++)
            {
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = new CellCoordinates(lowestX, floor),
                    position = RoomEntrance.Position.Left
                });
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = new CellCoordinates(highestX, floor),
                    position = RoomEntrance.Position.Right
                });
            }

            return result;
        }
    }
}