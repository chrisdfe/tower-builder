using TowerBuilder.DataTypes.Entities.Furnitures;

namespace TowerBuilder.DataTypes.Entities
{
    public class FurnitureEntity : Entity
    {
        public Furniture furniture { get; private set; }

        public FurnitureEntity(Furniture furniture)
        {
            this.furniture = furniture;
        }
    }
}