using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomCells
    {
        // public Room room { get; private set; }
        public List<RoomCell> cells { get; private set; }

        public delegate void RoomCellsEvent(RoomCells roomCells);
        public RoomCellsEvent onResize;

        public RoomCells()
        {
            // this.room = room;
            this.cells = new List<RoomCell>();
        }

        public RoomCells(int width, int height)
        {
            CreateRectangularRoom(width, height);
        }

        public RoomCells(CellCoordinates startCellCoordinates, CellCoordinates endCellCoordinates)
        {
            CreateRectangularRoom(startCellCoordinates, endCellCoordinates);
        }

        public RoomCells(List<RoomCell> roomCells)
        {
            this.cells = roomCells;
        }

        public void Add(CellCoordinates cellCoordinates)
        {
            cells.Add(new RoomCell(cellCoordinates));
            if (onResize != null)
            {
                onResize(this);
            }
        }

        public void Add(RoomCell roomCell)
        {
            cells.Add(new RoomCell(roomCell.coordinates));
            if (onResize != null)
            {
                onResize(this);
            }
        }

        public void Add(List<RoomCell> roomCellsList)
        {
            cells = cells.Concat(roomCellsList).ToList();

            if (onResize != null)
            {
                onResize(this);
            }
        }

        public void Add(RoomCells otherRoomCells)
        {
            Add(otherRoomCells.cells);
        }

        public void Set(RoomCells otherRoomCells)
        {
            this.cells = otherRoomCells.cells;
        }

        public void CreateRectangularRoom(int xWidth, int floors)
        {
            List<RoomCell> result = new List<RoomCell>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int floor = 0; floor < floors; floor++)
                {
                    result.Add(new RoomCell(x, floor));
                }
            }

            this.cells = result;

            if (onResize != null)
            {
                onResize(this);
            }
        }

        public void CreateRectangularRoom(CellCoordinates a, CellCoordinates b)
        {
            List<RoomCell> result = new List<RoomCell>();

            // startCoordinates = top left room
            // endCoordinates = bottom right room
            CellCoordinates startCoordinates = new CellCoordinates(Mathf.Min(a.x, b.x), Mathf.Min(a.floor, b.floor));
            CellCoordinates endCoordinates = new CellCoordinates(Mathf.Max(a.x, b.x), Mathf.Max(a.floor, b.floor));

            for (int floor = startCoordinates.floor; floor <= endCoordinates.floor; floor++)
            {
                for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
                {
                    result.Add(new RoomCell(x, floor));
                }
            }

            this.cells = result;

            if (onResize != null)
            {
                onResize(this);
            }
        }

        public void PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<RoomCell> result = new List<RoomCell>();

            foreach (RoomCell roomCell in cells)
            {
                RoomCell newRoomCell = new RoomCell(newBaseCoordinates.Add(roomCell.coordinates));
                result.Add(newRoomCell);
            }

            cells = result;
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

        public bool HasCellAtCoordinates(CellCoordinates cellCoordinates)
        {
            return FindCellByCoordinates(cellCoordinates) != null;
        }

        public int GetLowestX()
        {
            int lowestX = int.MaxValue;

            foreach (RoomCell roomCell in cells)
            {
                if (roomCell.coordinates.x < lowestX)
                {
                    lowestX = roomCell.coordinates.x;
                }
            }

            return lowestX;
        }

        public int GetHighestX()
        {
            int highestX = int.MinValue;

            foreach (RoomCell roomCell in cells)
            {
                if (roomCell.coordinates.x > highestX)
                {
                    highestX = roomCell.coordinates.x;
                }
            }

            return highestX;
        }


        public int GetLowestFloor()
        {
            int lowestFloor = int.MaxValue;

            foreach (RoomCell roomCell in cells)
            {
                if (roomCell.coordinates.floor < lowestFloor)
                {
                    lowestFloor = roomCell.coordinates.floor;
                }
            }

            return lowestFloor;
        }

        public int GetHighestFloor()
        {
            int highestFloor = int.MinValue;

            foreach (RoomCell roomCell in cells)
            {
                if (roomCell.coordinates.floor > highestFloor)
                {
                    highestFloor = roomCell.coordinates.floor;
                }
            }

            return highestFloor;
        }

        public List<int> GetXRange()
        {
            List<int> result = new List<int>();

            foreach (RoomCell roomCell in cells)
            {
                if (!result.Contains(roomCell.coordinates.x))
                {
                    result.Add(roomCell.coordinates.x);
                }
            }

            return result;
        }

        public List<int> GetFloorRange()
        {
            List<int> result = new List<int>();

            foreach (RoomCell roomCell in cells)
            {
                if (!result.Contains(roomCell.coordinates.floor))
                {
                    result.Add(roomCell.coordinates.floor);
                }
            }

            return result;
        }

        public RoomCells ToRelativeCoordinates()
        {
            List<RoomCell> result = new List<RoomCell>();
            CellCoordinates bottomLeftCoordinates = GetBottomLeftCoordinates();

            foreach (RoomCell roomCell in cells)
            {
                result.Add(new RoomCell(roomCell.coordinates.Subtract(bottomLeftCoordinates)));
            }

            return new RoomCells(result);
        }

        public CellCoordinates GetBottomLeftCoordinates()
        {
            return new CellCoordinates(
                GetLowestX(),
                GetLowestFloor()
            );
        }

        public CellCoordinates GetBottomRightCoordinates()
        {
            return new CellCoordinates(
                GetHighestX(),
                GetLowestFloor()
            );
        }

        public CellCoordinates GetTopLeftCoordinates()
        {
            return new CellCoordinates(
                GetLowestX(),
                GetHighestFloor()
            );
        }

        public CellCoordinates GetTopRightCoordinates()
        {
            return new CellCoordinates(
                GetHighestX(),
                GetHighestFloor()
            );
        }

        public int GetWidth()
        {
            int highestX = GetHighestX();
            int lowestX = GetLowestX();
            return (highestX - lowestX) + 1;
        }

        public int GetFloorSpan()
        {
            int highestFloor = GetHighestFloor();
            int lowestFloor = GetLowestFloor();
            return (highestFloor - lowestFloor) + 1;
        }
    }
}


