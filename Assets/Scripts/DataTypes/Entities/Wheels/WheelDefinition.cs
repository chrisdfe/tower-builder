using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class WheelDefinition : EntityDefinition<Wheel.Key>
    {
        public Wheel.SkinKey skinKey;

        public override ValidatorFactory validatorFactory => (Entity entity) => new WheelValidator(entity);
    }
}