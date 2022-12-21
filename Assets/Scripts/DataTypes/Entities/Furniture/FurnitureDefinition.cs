using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureDefinition : EntityDefinition<Furniture.Key>
    {
        // TODO - this should be a list of cellcoordinates instead
        public int homeSlotCount = 0;

        public delegate FurnitureBehaviorBase FurnitureBehaviorFactory(AppState appState, Furniture furniture);
        public FurnitureBehaviorFactory furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new DefaultBehavior(appState, furniture);

        public override ValidatorFactory validatorFactory => (Entity entity) => new FurnitureValidator(entity as Furniture);
    }
}