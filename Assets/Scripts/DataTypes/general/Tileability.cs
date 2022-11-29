using System.Collections.Generic;

namespace TowerBuilder.DataTypes
{
    public enum Tileability
    {
        Single,
        Horizontal,
        Vertical,
        Full
    }

    public enum TileablePosition
    {
        // Single (isolated)
        Single,

        // Horizontal
        Left, HorizontalCenter, Right,

        // Vertical
        Top, VerticalCenter, Bottom,

        // Full
        TopLeft, TopRight,
        Center,
        BottomLeft, BottomRight
    }

    public class OccupiedCellMap
    {
        public bool topLeft = false;
        public bool top = false;
        public bool topRight = false;

        public bool left = false;
        public bool right = false;

        public bool bottomLeft = false;
        public bool bottom = false;
        public bool bottomRight = false;
    }

    public class Tilabile
    {
        public static TileableBase FromTileability(Tileability tileability)
        {
            return tileability switch
            {
                Tileability.Single => new SingleTileable(),
                Tileability.Horizontal => new HorizontalTileable(),
                Tileability.Vertical => new VerticalTileable(),
                Tileability.Full => new FullyTilable(),
                _ => null
            };
        }
    }

    public abstract class TileableBase
    {
        public abstract Tileability tileability { get; }
        public abstract TileablePosition GetOrientation(OccupiedCellMap occupied);
    }

    public class SingleTileable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Single;
        public override TileablePosition GetOrientation(OccupiedCellMap occupied) { return TileablePosition.Single; }
    }

    public class HorizontalTileable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Single;

        public override TileablePosition GetOrientation(OccupiedCellMap occupied)
        {
            if (occupied.left && occupied.right)
            {
                return TileablePosition.HorizontalCenter;
            }

            if (occupied.left)
            {
                return TileablePosition.Right;
            }

            if (occupied.right)
            {
                return TileablePosition.Left;
            }

            return TileablePosition.Single;
        }
    }

    public class VerticalTileable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Single;

        public override TileablePosition GetOrientation(OccupiedCellMap occupied)
        {
            if (occupied.top && occupied.bottom)
            {
                return TileablePosition.VerticalCenter;
            }

            if (occupied.top)
            {
                return TileablePosition.Bottom;
            }

            if (occupied.bottom)
            {
                return TileablePosition.Top;
            }

            return TileablePosition.Single;
        }
    }

    public class FullyTilable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Full;

        public override TileablePosition GetOrientation(OccupiedCellMap occupied)
        {
            if (
                occupied.top &&
                occupied.right &&
                occupied.bottom &&
                occupied.left
            )
            {
                return TileablePosition.Center;
            }

            if (occupied.top && occupied.bottom)
            {
                return TileablePosition.VerticalCenter;
            }

            if (occupied.left && occupied.right)
            {
                return TileablePosition.HorizontalCenter;
            }

            // TODO - walls
            // TODO - corners

            if (occupied.left)
            {
                return TileablePosition.Right;
            }

            if (occupied.right)
            {
                return TileablePosition.Left;
            }

            if (occupied.top)
            {
                return TileablePosition.Bottom;
            }

            if (occupied.bottom)
            {
                return TileablePosition.Top;
            }


            return TileablePosition.Single;
        }
    }
}