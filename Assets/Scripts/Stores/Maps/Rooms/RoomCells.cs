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
        public List<CellCoordinates> cells { get; private set; }

        public RoomCells()
        {
            this.cells = new List<CellCoordinates>();
        }

        public RoomCells(int width, int height)
        {
            this.cells = RoomCells.CreateRectangularRoom(width, height);
        }

        public void Add(CellCoordinates cellCoordinates)
        {
            cells.Add(cellCoordinates);
        }

        public static List<CellCoordinates> CreateRectangularRoom(int xWidth, int floors)
        {
            List<CellCoordinates> roomCells = new List<CellCoordinates>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int floor = 0; floor < floors; floor++)
                {
                    roomCells.Add(new CellCoordinates()
                    {
                        x = x,
                        floor = floor
                    });
                }
            }

            return roomCells;
        }

        public static List<CellCoordinates> CreateRectangularRoom(CellCoordinates a, CellCoordinates b)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            CellCoordinates startCoordinates = new CellCoordinates(Mathf.Min(a.x, b.x), Mathf.Min(a.floor, b.floor));
            CellCoordinates endCoordinates = new CellCoordinates(Mathf.Max(a.x, b.x), Mathf.Max(a.floor, b.floor));

            for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
            {
                for (int floor = startCoordinates.floor; floor <= endCoordinates.floor; floor++)
                {
                    result.Add(new CellCoordinates(x, floor));
                }
            }

            return result;
        }


        public static List<CellCoordinates> PositionAtCoordinates(List<CellCoordinates> originalCellCoordinates, CellCoordinates targetCellCoordinates)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates coordinates in originalCellCoordinates)
            {
                result.Add(coordinates.Add(targetCellCoordinates));
            }

            return result;
        }
    }
}


