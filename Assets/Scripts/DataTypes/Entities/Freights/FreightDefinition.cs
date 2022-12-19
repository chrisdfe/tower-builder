using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightDefinition : EntityDefinition<FreightItem.Key>
    {
        public override ValidatorFactory validatorFactory => (Entity entity) => new FreightValidator(entity as FreightItem);
    }
}