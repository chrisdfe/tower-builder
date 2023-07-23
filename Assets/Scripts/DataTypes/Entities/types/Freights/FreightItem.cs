using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItem : Entity
    {
        public override string idKey => "freightItems";

        public FreightItem() : base() { }
        public FreightItem(Input input) : base(input) { }
        public FreightItem(FreightDefinition entityDefinition) : base(entityDefinition) { }
    }
}