using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Entities
{
    public class RoomBlockEntity : EntityBase
    {
        public RoomCells roomBlock { get; private set; }

        public RoomBlockEntity(RoomCells roomBlock)
        {
            this.roomBlock = roomBlock;
        }
    }
}