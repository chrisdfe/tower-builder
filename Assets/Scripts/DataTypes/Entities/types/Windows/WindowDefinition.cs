using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class WindowDefinition : EntityDefinition
    {
        public override ValidatorFactory buildValidatorFactory => (Entity entity) => new WindowValidator(entity as Window);
    }
}