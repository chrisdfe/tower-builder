

namespace TowerBuilder.DataTypes
{
    public struct Resizability
    {
        public bool x { get; private set; }
        public bool floor { get; private set; }

        public Resizability(bool x, bool floor)
        {
            this.x = x;
            this.floor = floor;
        }

        public bool Matches(Resizability b)
        {
            return Resizability.Matches(this, b);
        }

        public static bool Matches(Resizability a, Resizability b)
        {
            return a.x == b.x && a.floor == b.floor;
        }

        public static Resizability Inflexible()
        {
            return new Resizability(false, false);
        }

        public static Resizability Horizontal()
        {
            return new Resizability(true, false);
        }

        public static Resizability Vertical()
        {
            return new Resizability(false, true);
        }

        public static Resizability Flexible()
        {
            return new Resizability(true, true);

        }
    }
}


