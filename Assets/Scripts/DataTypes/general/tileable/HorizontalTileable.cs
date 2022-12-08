namespace TowerBuilder.DataTypes
{
    public class HorizontalTileable : Tileable
    {
        public override Type type { get; } = Type.Horizontal;

        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.Left,
                    CellPosition.HorizontalCenter,
                    CellPosition.Right
                };
        }

        public override CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (occupied.HasAll(new CellOrientation[] { CellOrientation.Left, CellOrientation.Right }))
            {
                return CellPosition.HorizontalCenter;
            }

            if (occupied.Has(CellOrientation.Left))
            {
                return CellPosition.Right;
            }

            if (occupied.Has(CellOrientation.Right))
            {
                return CellPosition.Left;
            }

            return CellPosition.Single;
        }
    }
}