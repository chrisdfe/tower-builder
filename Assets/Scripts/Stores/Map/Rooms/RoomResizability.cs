using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.Map.Rooms
{
    public struct RoomResizability
    {
        public bool x { get; private set; }
        public bool floor { get; private set; }

        public RoomResizability(bool x, bool floor)
        {
            this.x = x;
            this.floor = floor;
        }

        public bool Matches(RoomResizability b)
        {
            return RoomResizability.Matches(this, b);
        }

        public static bool Matches(RoomResizability a, RoomResizability b)
        {
            return a.x == b.x && a.floor == b.floor;
        }

        public static RoomResizability Inflexible()
        {
            return new RoomResizability(false, false);
        }

        public static RoomResizability Horizontal()
        {
            return new RoomResizability(true, false);
        }

        public static RoomResizability Vertical()
        {
            return new RoomResizability(false, true);
        }

        public static RoomResizability Flexible()
        {
            return new RoomResizability(true, true);

        }
    }
}


