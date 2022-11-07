using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    [Serializable]
    public class RoomCells
    {
        public List<RoomCell> cells = new List<RoomCell>();

        public int Count { get { return cells.Count; } }

        public CellCoordinatesList coordinatesList
        {
            get
            {
                return new CellCoordinatesList(cells.Select(roomCell => roomCell.coordinates).ToList());
            }
        }

        public RoomCells() { }

        public RoomCells(List<RoomCell> cells)
        {
            this.cells = cells;
            SetRoomCellOrientations();
        }

        public RoomCells(CellCoordinatesList cellCoordinatesList) : this(cellCoordinatesList.items.Select(cellCoordinates => new RoomCell(cellCoordinates)).ToList()) { }

        public void PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<RoomCell> result = new List<RoomCell>();

            foreach (RoomCell roomCell in cells)
            {
                RoomCell newRoomCell = new RoomCell(CellCoordinates.Add(newBaseCoordinates, roomCell.coordinates));
                result.Add(newRoomCell);
            }

            cells = result;
        }

        public CellCoordinates GetRelativeRoomCellCoordinates(RoomCell roomCell)
        {
            return CellCoordinates.Subtract(roomCell.coordinates, coordinatesList.GetBottomLeftCoordinates());
        }

        public RoomCell FindCellByCoordinates(CellCoordinates cellCoordinates)
        {
            foreach (RoomCell roomCell in cells)
            {
                if (roomCell.coordinates.Matches(cellCoordinates))
                {
                    return roomCell;
                }
            }

            return null;
        }

        void SetRoomCellOrientations()
        {
            foreach (RoomCell roomCell in cells)
            {
                SetRoomCellOrientation(roomCell);
            }
        }

        void SetRoomCellOrientation(RoomCell roomCell)
        {
            CellCoordinates coordinates = roomCell.coordinates;

            List<RoomCellOrientation> result = new List<RoomCellOrientation>();

            if (!coordinatesList.Contains(new CellCoordinates(coordinates.x, coordinates.floor + 1)))
            {
                result.Add(RoomCellOrientation.Top);
            }

            if (!coordinatesList.Contains(new CellCoordinates(coordinates.x + 1, coordinates.floor)))
            {
                result.Add(RoomCellOrientation.Right);
            }

            if (!coordinatesList.Contains(new CellCoordinates(coordinates.x, coordinates.floor - 1)))
            {
                result.Add(RoomCellOrientation.Bottom);
            }

            if (!coordinatesList.Contains(new CellCoordinates(coordinates.x - 1, coordinates.floor)))
            {
                result.Add(RoomCellOrientation.Left);
            }

            roomCell.orientation = result;
        }

        /* 
            Static API
         */
        public static RoomCells ToRelativeCoordinates(RoomCells roomCells)
        {
            List<RoomCell> result = new List<RoomCell>();
            CellCoordinates bottomLeftCoordinates = roomCells.coordinatesList.GetBottomLeftCoordinates();

            foreach (RoomCell roomCell in roomCells.cells)
            {
                result.Add(new RoomCell(CellCoordinates.Subtract(roomCell.coordinates, bottomLeftCoordinates)));
            }

            return new RoomCells(result);
        }
    }
}


