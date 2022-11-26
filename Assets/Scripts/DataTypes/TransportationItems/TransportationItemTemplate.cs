using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.TransportationItems
{
    public class TransportationItemTemplate
    {
        public string key = "None";
        public string title = "None";
        public string category = "None";

        public int pricePerCell = 0;

        public CellCoordinatesList cellCoordinatesList = new CellCoordinatesList(CellCoordinates.zero);
        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;
    }
}