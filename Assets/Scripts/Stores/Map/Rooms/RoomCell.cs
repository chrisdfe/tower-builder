using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomCell
    {
        // public RoomCells roomCells { get; private set; }
        public CellCoordinates coordinates = CellCoordinates.zero;
        public List<RoomEntrance> entrances = new List<RoomEntrance>();
        public List<RoomCellPosition> position = new List<RoomCellPosition>();


        public RoomCell(int x, int floor)
        {
            this.coordinates = new CellCoordinates(x, floor);
        }

        public RoomCell(CellCoordinates cellCoordinates)
        {
            this.coordinates = cellCoordinates.Clone();
        }
    }
}


