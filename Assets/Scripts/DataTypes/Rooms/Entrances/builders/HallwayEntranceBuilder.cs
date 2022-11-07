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
            int width = roomCells.coordinatesList.GetWidth();

            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.GetBottomLeftCoordinates(),
                position = RoomEntrancePosition.Left
            });


            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.GetBottomRightCoordinates(),
                position = RoomEntrancePosition.Right
            });


            return result;
        }
    }
}