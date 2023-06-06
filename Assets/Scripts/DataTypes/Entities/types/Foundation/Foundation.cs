using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.GameWorld.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class Foundation : Entity<Foundation.Key>
    {
        public enum Key
        {
            None,
            Default,
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" }
            }
        );

        public override string idKey { get => "foundations"; }

        public Room room;

        public Foundation(FoundationDefinition foundationDefinition) : base(foundationDefinition) { }
    }
}