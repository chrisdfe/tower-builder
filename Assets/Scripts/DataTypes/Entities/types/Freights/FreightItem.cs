using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItem : Entity
    {
        public override string idKey => "freightItems";

        public FreightItem(FreightDefinition entityDefinition) : base(entityDefinition) { }
    }
}