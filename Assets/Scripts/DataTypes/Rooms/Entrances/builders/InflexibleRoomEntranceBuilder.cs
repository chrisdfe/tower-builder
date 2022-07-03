using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes.Rooms.Entrances
{
    public class InflexibleRoomEntranceBuilder : RoomEntranceBuilderBase
    {
        public List<RoomEntrance> entrances;

        public InflexibleRoomEntranceBuilder(List<RoomEntrance> entrances)
        {
            this.entrances = entrances;
        }

        public override List<RoomEntrance> BuildRoomEntrances(RoomCells roomCells)
        {
            return entrances.Select(roomEntrance =>
            {
                // Convert cellCoordinates from relative to absolute
                RoomEntrance clonedRoomEntrance = roomEntrance.Clone();
                clonedRoomEntrance.cellCoordinates = roomEntrance.cellCoordinates.Add(roomCells.GetBottomLeftCoordinates());
                return clonedRoomEntrance;
            }).ToList();
        }
    }
}
