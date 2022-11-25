using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Rooms.Entrances
{
    public class HallwayEntranceBuilder : RoomEntranceBuilderBase
    {
        public override List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.coordinatesList.width;

            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.bottomLeftCoordinates,
                position = RoomEntrance.Position.Left
            });


            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.bottomRightCoordinates,
                position = RoomEntrance.Position.Right
            });


            return result;
        }
    }
}