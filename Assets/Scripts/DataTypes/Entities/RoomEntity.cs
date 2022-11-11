using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Entities
{
    public class RoomEntity : EntityBase
    {
        public Room room { get; private set; }

        public RoomEntity(Room room)
        {
            this.room = room;
        }
    }
}