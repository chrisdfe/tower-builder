using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities.Rooms;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureList : ListWrapper<Furniture>
    {
        public FurnitureList() { }
        public FurnitureList(Furniture furniture) : base(furniture) { }
        public FurnitureList(List<Furniture> furnitureList) : base(furnitureList) { }
        public FurnitureList(FurnitureList furnitureList) : base(furnitureList.items) { }


        public Furniture FindFurnitureAtCell(CellCoordinates cellCoordinates) =>
            items.Find(furniture => furniture.cellCoordinatesList.Contains(cellCoordinates));
    }
}