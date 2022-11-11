using TowerBuilder.DataTypes.Furnitures;

namespace TowerBuilder.DataTypes.Entities
{
    public class FurnitureEntity : EntityBase
    {
        public Furniture furniture { get; private set; }

        public FurnitureEntity(Furniture furniture)
        {
            this.furniture = furniture;
        }
    }
}