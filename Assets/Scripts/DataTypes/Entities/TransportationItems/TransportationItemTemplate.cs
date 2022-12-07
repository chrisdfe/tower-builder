using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemTemplate : EntityTemplate<TransportationItem.Key>
    {
        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;
    }
}