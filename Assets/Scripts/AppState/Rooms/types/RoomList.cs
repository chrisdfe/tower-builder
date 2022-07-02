using System;
using System.Collections.Generic;

namespace TowerBuilder.State.Rooms
{
    [Serializable]
    public class RoomList
    {
        public List<Room> rooms { get; private set; }

        public RoomList()
        {
            rooms = new List<Room>();
        }

        public RoomList(List<Room> rooms)
        {
            this.rooms = rooms;
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
            foreach (Room room in rooms)
            {
                foreach (RoomCell roomCell in room.roomCells.cells)
                {
                    if (roomCell.coordinates.Matches(targetCellCoordinates))
                    {
                        return room;
                    }
                }
            }

            return null;
        }

        public Room FindRoomByRoomBlock(RoomCells roomBlock)
        {
            foreach (Room room in rooms)
            {
                if (room.ContainsBlock(roomBlock))
                {
                    return room;
                }
            }

            return null;
        }

        public bool Contains(Room room)
        {
            return rooms.Contains(room);
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
            // List<MapRoom> mapRooms = Registry.appState.Rooms.mapRooms;

            foreach (Room room in rooms)
            {
                foreach (RoomCell roomCell in room.roomCells.cells)
                {
                    if (cellCoordinates.Matches(roomCell.coordinates))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}


