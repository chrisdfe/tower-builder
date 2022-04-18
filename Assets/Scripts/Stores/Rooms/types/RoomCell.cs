using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms
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
        public RoomCells parent { get; private set; }

        public CellCoordinates coordinates = CellCoordinates.zero;
        public List<RoomEntrance> entrances = new List<RoomEntrance>();
        public List<RoomCellOrientation> orientation = new List<RoomCellOrientation>();

        public bool hasFloor { get; private set; } = true;

        public RoomCell(RoomCells parent, int x, int floor)
        {
            this.parent = parent;
            this.coordinates = new CellCoordinates(x, floor);
        }

        public RoomCell(RoomCells parent, CellCoordinates cellCoordinates)
        {
            this.parent = parent;
            this.coordinates = cellCoordinates.Clone();
        }

        public CellCoordinates GetRelativeCoordinates()
        {
            return this.coordinates.Subtract(this.parent.GetBottomLeftCoordinates());
        }
    }
}


