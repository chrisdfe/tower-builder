using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.InteriorWalls
{
    public class InteriorWall : Entity<InteriorWall.Key>
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

        public override string idKey { get => "interiorWall"; }

        public override Type type => Entity.Type.InteriorWall;

        public InteriorWall(InteriorWallDefinition interiorWallDefinition) : base(interiorWallDefinition) { }
    }
}