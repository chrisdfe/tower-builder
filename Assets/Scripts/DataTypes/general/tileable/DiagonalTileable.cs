namespace TowerBuilder.DataTypes
{
    public class DiagonalTileable : Tileable
    {
        public override Type type { get; } = Type.Diagonal;

        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.BottomLeft,
                    CellPosition.DiagonalCenter,
                    CellPosition.TopRight
                };
        }

        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (occupied.HasAll(new CellOrientation[] { CellOrientation.BelowLeft, CellOrientation.AboveRight }))
            {
                return Tileable.CellPosition.DiagonalCenter;
            }

            if (occupied.Has(CellOrientation.AboveRight))
            {
                return Tileable.CellPosition.BottomLeft;
            }

            if (occupied.Has(CellOrientation.BelowLeft))
            {
                return Tileable.CellPosition.TopRight;
            }

            return Tileable.CellPosition.Single;
        }
    }
}