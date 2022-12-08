namespace TowerBuilder.DataTypes
{
    public class VerticalTileable : Tileable
    {
        public override Type type { get; } = Type.Vertical;

        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.Top,
                    CellPosition.VerticalCenter,
                    CellPosition.Bottom
                };
        }

        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (occupied.HasAll(new CellOrientation[] { CellOrientation.Above, CellOrientation.Below }))
            {
                return Tileable.CellPosition.VerticalCenter;
            }

            if (occupied.Has(CellOrientation.Above))
            {
                return Tileable.CellPosition.Bottom;
            }

            if (occupied.Has(CellOrientation.Below))
            {
                return Tileable.CellPosition.Top;
            }

            return Tileable.CellPosition.Single;
        }
    }
}