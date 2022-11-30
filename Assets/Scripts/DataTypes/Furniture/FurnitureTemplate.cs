using System.Collections.Generic;
using TowerBuilder.ApplicationState;
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
        public int homeSlotCount = 0;

        public delegate FurnitureBehaviorBase FurnitureBehaviorFactory(AppState appState, Furniture furniture);
        public FurnitureBehaviorFactory furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new DefaultBehavior(appState, furniture);

        public delegate FurnitureValidatorBase FurnitureValidatorFactory(Furniture furniture);
        public FurnitureValidatorFactory furnitureValidatorFactory = (Furniture furniture) => new DefaultFurnitureValidator(furniture);
    }
}