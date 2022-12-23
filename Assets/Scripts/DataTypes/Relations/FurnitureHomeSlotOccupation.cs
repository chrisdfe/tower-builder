using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.DataTypes.Relations
{
    public class FurnitureHomeSlotOccupation
    {
        public Resident resident;
        public Furniture furniture;
    }
}