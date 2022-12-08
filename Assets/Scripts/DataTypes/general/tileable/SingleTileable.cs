namespace TowerBuilder.DataTypes
{
    public class SingleTileable : Tileable
    {
        public override Type type { get; } = Type.Single;
        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied) => CellPosition.Single;

        public override CellPosition[] allPossibleCellPositions
        {
            get => new CellPosition[] { CellPosition.Single };
        }
    }
}