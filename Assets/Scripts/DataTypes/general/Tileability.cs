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

    public enum OccupiedCell
    {
        TopLeft,
        Top,
        TopRight,

        Left,
        Right,

        BottomLeft,
        Bottom,
        BottomRight
    }

    public abstract class TileableBase
    {
        public abstract Tileability tileability { get; }
        public abstract TileablePosition GetOrientation(List<OccupiedCell> occupied);
    }

    public class SingleTileable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Single;
        public override TileablePosition GetOrientation(List<OccupiedCell> occupied) { return TileablePosition.Single; }
    }

    public class HorizontalTileable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Single;

        public override TileablePosition GetOrientation(List<OccupiedCell> occupied)
        {
            if (occupied.Contains(OccupiedCell.Left) && occupied.Contains(OccupiedCell.Right))
            {
                return TileablePosition.HorizontalCenter;
            }

            if (occupied.Contains(OccupiedCell.Left))
            {
                return TileablePosition.Right;
            }

            if (occupied.Contains(OccupiedCell.Right))
            {
                return TileablePosition.Left;
            }

            return TileablePosition.Single;
        }
    }

    public class VerticalTileable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Single;

        public override TileablePosition GetOrientation(List<OccupiedCell> occupied)
        {
            if (occupied.Contains(OccupiedCell.Top) && occupied.Contains(OccupiedCell.Bottom))
            {
                return TileablePosition.VerticalCenter;
            }

            if (occupied.Contains(OccupiedCell.Top))
            {
                return TileablePosition.Bottom;
            }

            if (occupied.Contains(OccupiedCell.Bottom))
            {
                return TileablePosition.Top;
            }

            return TileablePosition.Single;
        }
    }

    public class FullyTilable : TileableBase
    {
        public override Tileability tileability { get; } = Tileability.Full;

        public override TileablePosition GetOrientation(List<OccupiedCell> occupied)
        {
            if (
                occupied.Contains(OccupiedCell.Top) &&
                occupied.Contains(OccupiedCell.Right) &&
                occupied.Contains(OccupiedCell.Bottom) &&
                occupied.Contains(OccupiedCell.Left)
            )
            {
                return TileablePosition.Center;
            }

            if (occupied.Contains(OccupiedCell.Top) && occupied.Contains(OccupiedCell.Bottom))
            {
                return TileablePosition.VerticalCenter;
            }

            if (occupied.Contains(OccupiedCell.Left) && occupied.Contains(OccupiedCell.Right))
            {
                return TileablePosition.HorizontalCenter;
            }

            // TODO - walls
            // TODO - corners

            if (occupied.Contains(OccupiedCell.Left))
            {
                return TileablePosition.Right;
            }

            if (occupied.Contains(OccupiedCell.Right))
            {
                return TileablePosition.Left;
            }

            if (occupied.Contains(OccupiedCell.Top))
            {
                return TileablePosition.Bottom;
            }

            if (occupied.Contains(OccupiedCell.Bottom))
            {
                return TileablePosition.Top;
            }


            return TileablePosition.Single;
        }
    }
}