namespace TowerBuilder.Stores.Map.Rooms.Modules
{
    public class ResidenceDetails : RoomModuleDetailsBase
    {
        public override RoomModuleKey roomModuleKey { get { return RoomModuleKey.Residence; } }

        // # of residents that can call this room home
        public int occupancy;
    }
}