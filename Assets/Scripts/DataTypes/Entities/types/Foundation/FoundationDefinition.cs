using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationDefinition : EntityDefinition
    {
        public override ValidatorFactory buildValidatorFactory => (Entity entity) => new FoundationBuildValidator(entity as Foundation);
        public override ValidatorFactory destroyValidatorFactory => (Entity entity) => new FoundationDestroyValidator(entity as Foundation);
    }
}