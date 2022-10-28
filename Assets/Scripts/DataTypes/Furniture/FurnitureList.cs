using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Furniture
{
    public class FurnitureList
    {
        public List<FurnitureBase> furnitureList { get; private set; } = new List<FurnitureBase>();
    }
}