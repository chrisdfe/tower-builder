namespace TowerBuilder.Stores.Map.Rooms.Uses
{
    public class ElevatorDetails : RoomUseDetailsBase
    {
        public override RoomUseKey roomUseKey { get { return RoomUseKey.Elevator; } }

        // # of residents that can fit in an elevator car
        public int capacity;

        // how fast elevator cars move between each floor
        public int speed;
    }
}