using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class WheelValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>() {
                GenericEntityValidations.CreateValidateEntityIsOnFloor(0)
            };

        public WheelValidator(Entity entity) : base(entity) { }
    }
}