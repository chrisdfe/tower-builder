using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class WindowDefinition : EntityDefinition<Window.Key>
    {
        public override ValidatorFactory validatorFactory => (Entity entity) => new WindowValidator(entity as Window);
    }
}