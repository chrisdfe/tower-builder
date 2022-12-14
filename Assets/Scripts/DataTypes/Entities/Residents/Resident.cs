using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class Resident : Entity<Resident.Key>
    {
        public enum Key
        {
            Default,
            OtherDefault,
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" },
                { Key.OtherDefault,  "OtherDefault" },
            }
        );

        public override string idKey => "Residents";

        public override Type type => Entity.Type.Resident;

        public Resident(ResidentDefinition definition) : base(definition) { }

        public override string ToString() => $"Resident {id}";
    }
}