using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItem : Entity<FreightItem.Key>
    {
        public enum Key
        {
            None,
            Small,
            Medium,
            Large
        }

        public FreightItem(EntityTemplate<FreightItem.Key> entityTemplate) : base(entityTemplate) { }
    }
}