namespace TowerBuilder.Stores.Map.Rooms.Uses
{
    public class ResidenceDetails : RoomUseDetailsBase
    {
        public override RoomUseKey roomUseKey { get { return RoomUseKey.Residence; } }

        // # of residents that can call this room home
        public int occupancy;
    }
}