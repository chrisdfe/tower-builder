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
        public Room room { get; private set; }
        public List<RoomCell> cells { get; private set; }

        public RoomCells(Room room)
        {
            this.room = room;
            this.cells = new List<RoomCell>();
        }

        public RoomCells(Room room, int width, int height) : this(room)
        {
            CreateRectangularRoom(width, height);
        }

        public void SetRoom(Room room)
        {
            this.room = room;

            foreach (RoomCell cell in cells)
            {
                cell.SetRoom(room);
            }
        }

        public void Add(CellCoordinates cellCoordinates)
        {
            cells.Add(new RoomCell(this.room)
            {
                coordinates = cellCoordinates
            });
        }

        public void Add(RoomCell roomCell)
        {
            roomCell.SetRoom(room);
            cells.Add(roomCell);
        }

        public void Add(RoomCells roomCells)
        {
            cells = cells.Concat(roomCells.cells).ToList();
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

        public void CreateRectangularRoom(int xWidth, int floors)
        {
            List<RoomCell> roomCells = new List<RoomCell>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int floor = 0; floor < floors; floor++)
                {
                    roomCells.Add(new RoomCell(room, x, floor));
                }
            }

            this.cells = roomCells;
        }

        public void CreateRectangularRoom(CellCoordinates a, CellCoordinates b)
        {
            List<RoomCell> roomCells = new List<RoomCell>();

            // startCoordinates = top left room
            // endCoordinates = bottom right room
            CellCoordinates startCoordinates = new CellCoordinates(Mathf.Min(a.x, b.x), Mathf.Min(a.floor, b.floor));
            CellCoordinates endCoordinates = new CellCoordinates(Mathf.Max(a.x, b.x), Mathf.Max(a.floor, b.floor));

            for (int floor = startCoordinates.floor; floor <= endCoordinates.floor; floor++)
            {
                for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
                {
                    roomCells.Add(new RoomCell(room, x, floor));
                }
            }

            this.cells = roomCells;
        }

        public void PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<RoomCell> result = new List<RoomCell>();

            foreach (RoomCell roomCell in cells)
            {
                result.Add(
                    new RoomCell(room)
                    {
                        coordinates = newBaseCoordinates.Add(roomCell.coordinates)
                    }
                );
            }

            cells = result;
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

        public List<int> getXRange()
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

        public List<int> getFloorRange()
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


