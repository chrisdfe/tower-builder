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
            int width = roomCells.coordinatesList.width - 1;

            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.bottomLeftCoordinates.Clone(),
                position = RoomEntrancePosition.Left
            });
            result.Add(new RoomEntrance()
            {
                cellCoordinates = roomCells.coordinatesList.bottomRightCoordinates.Clone(),
                position = RoomEntrancePosition.Right
            });

            int floorSpan = roomCells.coordinatesList.floorSpan;
            if (floorSpan > 1)
            {
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = roomCells.coordinatesList.topLeftCoordinates,
                    position = RoomEntrancePosition.Left
                });
                result.Add(new RoomEntrance()
                {
                    cellCoordinates = roomCells.coordinatesList.topRightCoordinates,
                    position = RoomEntrancePosition.Right
                });
            }

            return result;
        }
    }
}