using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationDefinition : EntityDefinition
    {
        public override ValidatorFactory validatorFactory => (Entity entity) => new FoundationValidator(entity as Foundation);
    }
}