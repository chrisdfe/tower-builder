namespace TowerBuilder.Stores.Map.Rooms.Uses
{
    public class ResidenceDetails : RoomUseDetailsBase
    {
        public override RoomUseKey roomUseKey { get { return RoomUseKey.Residence; } }
        public int occupancy;
    }
}