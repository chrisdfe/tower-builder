using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities.Rooms;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorList : ListWrapper<Floor>
    {
        public FloorList() { }
        public FloorList(Floor floor) : base(floor) { }
        public FloorList(List<Floor> floorList) : base(floorList) { }
        public FloorList(FloorList floorList) : base(floorList.items) { }

        public Floor FindFloorAtCell(CellCoordinates cellCoordinates) =>
            items.Find(floor => floor.cellCoordinatesList.Contains(cellCoordinates));
    }
}