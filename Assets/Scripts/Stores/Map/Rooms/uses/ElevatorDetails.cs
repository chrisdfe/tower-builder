namespace TowerBuilder.Stores.Map.Rooms.Uses
{
    public class ElevatorDetails : RoomUseDetailsBase
    {
        public override RoomUseKey roomUseKey { get { return RoomUseKey.Elevator; } }
        public int capacity;
    }
}