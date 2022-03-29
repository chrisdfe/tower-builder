using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    // These can be combined to create corners,
    // e.g {Top, Left} or {Bottom,Right}
    // or tunnels
    // e.g {Top, Bottom} or {Left, Right}
    // or tunnel end points
    // e.g {Top, Left, Right}
    public enum RoomCellOrientation
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    public class RoomCell
    {
        // public RoomCells roomCells { get; private set; }
        public CellCoordinates coordinates = CellCoordinates.zero;
        public List<RoomEntrance> entrances = new List<RoomEntrance>();
        public List<RoomCellOrientation> orientation = new List<RoomCellOrientation>();


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


