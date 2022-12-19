using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public TransportationItemValidator(TransportationItem transportationItem) : base(transportationItem) { }

    }
}