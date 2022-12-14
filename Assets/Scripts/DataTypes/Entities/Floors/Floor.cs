using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class Floor : Entity<Floor.Key>
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

        public override string idKey { get => "floors"; }

        public override Type type => Entity.Type.Floor;

        public Floor(FloorDefinition floorDefinition) : base(floorDefinition) { }
    }
}