using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomEntrance
    {
        public RoomCell roomCell;
        public RoomEntrancePosition position;
        public CellCoordinates cellCoordinates;

        public RoomEntrance Clone()
        {
            return new RoomEntrance()
            {
                roomCell = this.roomCell,
                position = this.position,
                cellCoordinates = this.cellCoordinates
            };
        }
    }
}


