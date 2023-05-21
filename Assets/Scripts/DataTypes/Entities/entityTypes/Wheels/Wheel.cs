using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class Wheel : Entity<Wheel.Key>
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

        public enum SkinKey
        {
            Default,
        }

        public override string idKey { get => "wheels"; }

        public SkinKey skinKey;

        public Wheel(WheelDefinition wheelDefinition) : base(wheelDefinition)
        {
            this.skinKey = wheelDefinition.skinKey;
        }
    }
}