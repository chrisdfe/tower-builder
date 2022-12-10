using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemDefinition : EntityDefinition<TransportationItem.Key>
    {
        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;
    }
}