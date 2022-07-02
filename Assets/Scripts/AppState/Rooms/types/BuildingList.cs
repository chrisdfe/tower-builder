using System.Collections.Generic;
using System.Linq;
using TowerBuilder.State.Rooms;

namespace TowerBuilder.State.Rooms
{
    public class BuildingList
    {
        public List<Building> buildings { get; private set; } = new List<Building>();

        // Creation
        public void Add(Building building)
        {
            buildings.Add(building);
        }

        // Queries
        public Building FindBuildingByRoom(Room room)
        {
            foreach (Building building in buildings)
            {
                if (building.ContainsRoom(room))
                {
                    return building;
                }
            }
            return null;
        }

        public RoomList FindAllRooms()
        {
            List<Room> roomsList = new List<Room>();

            foreach (Building building in buildings)
            {
                roomsList = roomsList.Concat(building.roomList.rooms).ToList();
            }

            return new RoomList(roomsList);
        }

        public Room FindRoomAtCell(CellCoordinates cellCoordinates)
        {
            foreach (Building building in buildings)
            {
                Room room = building.roomList.FindRoomAtCell(cellCoordinates);

                if (room != null)
                {
                    return room;
                }
            }

            return null;
        }
    }
}