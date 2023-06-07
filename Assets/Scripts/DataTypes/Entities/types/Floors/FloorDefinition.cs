using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorDefinition : EntityDefinition
    {
        public override ValidatorFactory validatorFactory => (Entity entity) => new FloorValidator(entity as Floor);
    }
}