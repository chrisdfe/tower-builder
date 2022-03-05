using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class RoomCells
    {
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

            Debug.Log("startCoordinates: " + startCoordinates);
            Debug.Log("endCoordinates: " + endCoordinates);

            for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
            {
                for (int floor = startCoordinates.floor; floor <= endCoordinates.floor; floor++)
                {
                    result.Add(new CellCoordinates(x, floor));
                }
            }

            Debug.Log("result: " + result.Count);

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

        public static bool CellIsOccupied(CellCoordinates cellCoordinates, List<MapRoom> mapRooms)
        {
            // List<MapRoom> mapRooms = Registry.Stores.Map.mapRooms;

            foreach (MapRoom mapRoom in mapRooms)
            {
                foreach (CellCoordinates roomCellCoordiantes in mapRoom.roomCells)
                {
                    if (cellCoordinates.Matches(roomCellCoordiantes))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}


