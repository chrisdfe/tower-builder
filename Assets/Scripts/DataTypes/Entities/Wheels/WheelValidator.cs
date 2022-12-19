using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class WheelValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>() {
                GenericEntityValidations.CreateValidateEntityIsOnFloor(0)
            };

        public WheelValidator(Entity entity) : base(entity) { }
    }
}