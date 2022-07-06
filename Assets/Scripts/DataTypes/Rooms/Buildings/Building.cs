namespace TowerBuilder.DataTypes.Rooms.Buildings
{
    public class Building
    {
        public BuildingType type = BuildingType.Static;
        public RoomList roomList = new RoomList();

        public bool ContainsRoom(Room room)
        {
            return roomList.Contains(room);
        }

        public void AddRoom(Room room)
        {
            roomList.Add(room);
        }

        public void RemoveRoom(Room room)
        {
            roomList.Remove(room);
        }
    }
}