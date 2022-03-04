using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class RoomCells
    {
        public List<CellCoordinates> cells { get; private set; }

        public RoomCells(List<CellCoordinates> roomCells)
        {
            cells = roomCells;
        }

        public static RoomCells CreateRectangularRoom(int xWidth, int floors)
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

            return new RoomCells(roomCells);
        }

        public static RoomCells PositionAtCoordinates(RoomCells roomCells, CellCoordinates targetCellCoordinates)
        {
            List<CellCoordinates> newCells = new List<CellCoordinates>();

            foreach (CellCoordinates coordinates in roomCells.cells)
            {
                newCells.Add(new CellCoordinates()
                {
                    x = coordinates.x + targetCellCoordinates.x,
                    floor = coordinates.floor + targetCellCoordinates.floor
                });
            }

            // TODO - does this mutate roomCells outside of the scope of this method?
            roomCells.cells = newCells;
            return roomCells;
        }

        public static int GetLowestX(List<CellCoordinates> roomCells)
        {
            int lowestX = int.MaxValue;

            foreach (CellCoordinates cellCoordinates in roomCells)
            {
                if (cellCoordinates.x < lowestX)
                {
                    lowestX = cellCoordinates.x;
                }
            }

            return lowestX;
        }


        public static int GetHighestX(List<CellCoordinates> roomCells)
        {
            int highestX = int.MinValue;

            foreach (CellCoordinates cellCoordinates in roomCells)
            {
                if (cellCoordinates.x > highestX)
                {
                    highestX = cellCoordinates.x;
                }
            }

            return highestX;
        }

        // TODO - same for floor
    }
}


