using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureDefinition : EntityDefinition
    {
        // TODO - this should be a list of cellcoordinates instead
        public int homeSlotCount = 0;

        public delegate FurnitureBehavior FurnitureBehaviorFactory(AppState appState, Furniture furniture);
        public FurnitureBehaviorFactory furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new DefaultBehavior(appState, furniture);

        public override ValidatorFactory buildValidatorFactory => (Entity entity) => new FurnitureValidator(entity as Furniture);
    }
}