using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class WindowDefinition : EntityDefinition
    {
        public override ValidatorFactory validatorFactory => (Entity entity) => new WindowValidator(entity as Window);
    }
}