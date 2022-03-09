using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomList
    {
        public List<Room> rooms { get; private set; }

        public RoomList()
        {
            rooms = new List<Room>();
        }


        public void Add(Room room)
        {
            rooms.Add(room);
        }

        public void Remove(Room room)
        {
            rooms.Remove(room);
        }

        public Room FindRoomAtCell(CellCoordinates targetCellCoordinates)
        {
            return RoomList.FindRoomAtCell(targetCellCoordinates, rooms);
        }

        // TODO - remove this method
        public static Room FindRoomAtCell(CellCoordinates targetCellCoordinates, List<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                foreach (CellCoordinates cellCoordinates in room.roomCells.cells)
                {
                    if (cellCoordinates.Matches(targetCellCoordinates))
                    {
                        return room;
                    }
                }
            }

            return null;
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

        public static bool CellIsOccupied(CellCoordinates cellCoordinates, List<Room> rooms)
        {
            // List<MapRoom> mapRooms = Registry.Stores.Map.mapRooms;

            foreach (Room room in rooms)
            {
                foreach (CellCoordinates roomCellCoordiantes in room.roomCells.cells)
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


