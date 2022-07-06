using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Furniture
{
    public class FurnitureList
    {
        List<FurnitureBase> furnitureList = new List<FurnitureBase>();

        public int Count
        {
            get
            {
                return furnitureList.Count;
            }
        }
    }
}