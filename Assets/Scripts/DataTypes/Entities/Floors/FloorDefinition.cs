using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorDefinition : EntityDefinition<Floor.Key>
    {
        public override ValidatorFactory validatorFactory => (Entity entity) => new FloorValidator(entity as Floor);
    }
}