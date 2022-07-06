using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Rooms.Buildings
{
    public class BuildingList
    {
        public List<Building> buildings { get; private set; } = new List<Building>();

        public int Count
        {
            get
            {
                return buildings.Count;
            }
        }

        // Creation
        public void Add(Building building)
        {
            buildings.Add(building);
        }

        // Deletion
        public void Remove(Building building)
        {
            buildings.Remove(building);
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