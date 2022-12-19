using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightItem : Entity<FreightItem.Key>
    {
        public enum Key
        {
            // None,
            Small,
            Medium,
            Large
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                // { Key.None,   "None" },
                { Key.Small,  "Small" },
                { Key.Medium, "Medium" },
                { Key.Large,  "Large" },
            }
        );

        public override string idKey => "freightItems";

        public override Type type => Entity.Type.Freight;

        public FreightItem(FreightDefinition entityDefinition) : base(entityDefinition) { }
    }
}