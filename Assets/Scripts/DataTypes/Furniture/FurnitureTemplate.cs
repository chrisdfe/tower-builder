using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Furnitures.Validators;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class FurnitureTemplate
    {
        public string key = "None";
        public string title = "None";
        public string category = "None";

        public int price = 0;

        public delegate FurnitureBehaviorBase FurnitureBehaviorFactory(Furniture furniture);
        public FurnitureBehaviorFactory furnitureBehaviorFactory = (Furniture furniture) => new DefaultBehavior(furniture);

        public delegate FurnitureValidatorBase FurnitureValidatorFactory(Furniture furniture);
        public FurnitureValidatorFactory furnitureValidatorFactory = (Furniture furniture) => new DefaultFurnitureValidator(furniture);
    }
}