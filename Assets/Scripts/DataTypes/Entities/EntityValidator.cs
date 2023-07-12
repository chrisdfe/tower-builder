using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityValidator : Validator<Entity>
    {
        public override List<ValidationFunc> baseValidators => new List<ValidationFunc>() {
            GenericEntityValidations.ValidateEntityIsNotOverlappingAnotherEntityOfSameType
        };

        public EntityValidator(Entity entity) : base(entity) { }
    }
}