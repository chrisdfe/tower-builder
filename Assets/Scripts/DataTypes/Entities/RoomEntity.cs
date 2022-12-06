using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Entities
{
    public class RoomEntity : Entity
    {
        public Room room { get; private set; }

        public RoomEntity(Room room)
        {
            this.room = room;
        }
    }
}