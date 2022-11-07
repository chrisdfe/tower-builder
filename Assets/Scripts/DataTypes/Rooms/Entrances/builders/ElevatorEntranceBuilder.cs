using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Rooms.Entrances
{
    public class ElevatorEntranceBuilder : RoomEntranceBuilderBase
    {
        public override List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            List<RoomEntrance> result = new List<RoomEntrance>();
            int width = roomCells.coordinatesList.GetWidth() - 1;

            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.GetBottomLeftCoordinates().Clone(),
                position = RoomEntrancePosition.Left
            });
            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.GetBottomRightCoordinates().Clone(),
                position = RoomEntrancePosition.Right
            });

            int floorSpan = roomCells.coordinatesList.GetFloorSpan();
            if (floorSpan > 1)
            {
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = roomCells.coordinatesList.GetTopLeftCoordinates(),
                    position = RoomEntrancePosition.Left
                });
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = roomCells.coordinatesList.GetTopRightCoordinates(),
                    position = RoomEntrancePosition.Right
                });
            }

            return result;
        }
    }
}