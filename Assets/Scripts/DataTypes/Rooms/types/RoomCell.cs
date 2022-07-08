using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomCell
    {
        [NonSerialized]
        public RoomCells parent;

        public CellCoordinates coordinates = CellCoordinates.zero;

        [NonSerialized]
        public List<RoomCellOrientation> orientation = new List<RoomCellOrientation>();

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

        public CellCoordinates GetCoordinatesAbove()
        {
            return new CellCoordinates(coordinates.x, coordinates.floor + 1);
        }

        public CellCoordinates GetCoordinatesBelow()
        {
            return new CellCoordinates(coordinates.x, coordinates.floor - 1);
        }

        public CellCoordinates GetCoordinatesLeft()
        {
            return new CellCoordinates(coordinates.x - 1, coordinates.floor);
        }

        public CellCoordinates GetCoordinatesRight()
        {
            return new CellCoordinates(coordinates.x + 1, coordinates.floor);
        }
    }
}


