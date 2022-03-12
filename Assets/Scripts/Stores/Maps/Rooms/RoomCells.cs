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
        public List<RoomCell> cells { get; private set; }

        public RoomCells()
        {
            this.cells = new List<RoomCell>();
        }

        public RoomCells(int width, int height)
        {
            this.cells = RoomCells.CreateRectangularRoom(width, height);
        }

        public void Add(CellCoordinates cellCoordinates)
        {
            cells.Add(new RoomCell()
            {
                coordinates = cellCoordinates
            });
        }

        public void Add(RoomCell roomCell)
        {
            cells.Add(roomCell);
        }

        public void Add(RoomCells roomCells)
        {
            cells = cells.Concat(roomCells.cells).ToList();
        }

        public static List<RoomCell> CreateRectangularRoom(int xWidth, int floors)
        {
            List<RoomCell> roomCells = new List<RoomCell>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int floor = 0; floor < floors; floor++)
                {
                    roomCells.Add(new RoomCell()
                    {
                        coordinates = new CellCoordinates()
                        {
                            x = x,
                            floor = floor
                        }
                    });
                }
            }

            return roomCells;
        }

        public static RoomCells CreateRectangularRoom(CellCoordinates a, CellCoordinates b)
        {
            List<RoomCell> result = new List<RoomCell>();

            CellCoordinates startCoordinates = new CellCoordinates(Mathf.Min(a.x, b.x), Mathf.Min(a.floor, b.floor));
            CellCoordinates endCoordinates = new CellCoordinates(Mathf.Max(a.x, b.x), Mathf.Max(a.floor, b.floor));

            for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
            {
                for (int floor = startCoordinates.floor; floor <= endCoordinates.floor; floor++)
                {
                    result.Add(new RoomCell()
                    {
                        coordinates = new CellCoordinates(x, floor)
                    });
                }
            }

            RoomCells roomCells = new RoomCells()
            {
                cells = result
            };

            return roomCells;
        }

        public RoomCells PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<RoomCell> result = new List<RoomCell>();

            foreach (RoomCell roomCell in cells)
            {
                result.Add(
                    new RoomCell()
                    {
                        coordinates = newBaseCoordinates.Add(roomCell.coordinates)
                    }
                );
            }

            cells = result;
            return this;
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


