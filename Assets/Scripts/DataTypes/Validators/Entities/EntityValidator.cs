using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Validators.Entities
{
    public class EntityValidator : Validator<Entity>
    {
        public override List<ValidationFunc> baseValidators =>
            new List<ValidationFunc>() {
                GenericEntityValidations.ValidateWalletHasEnoughMoney,
                GenericEntityValidations.ValidateEntityIsNotOverlappingAnotherEntity,
                // TODO - put this in a vehicle-specific validation set
                GenericEntityValidations.ValidateEntityIsNotUnderground,
            };

        Entity validationEntity => validationItem as Entity;

        public EntityValidator(Entity entity) : base(entity) { }
    }
}