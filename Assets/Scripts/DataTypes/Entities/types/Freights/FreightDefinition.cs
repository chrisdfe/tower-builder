using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightDefinition : EntityDefinition
    {
        public override ValidatorFactory buildValidatorFactory => (Entity entity) => new FreightValidator(entity as FreightItem);
    }
}