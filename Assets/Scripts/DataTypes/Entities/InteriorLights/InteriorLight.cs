using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.InteriorLights
{
    public class InteriorLight : Entity<InteriorLight.Key>
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

        public override string idKey { get => "interiorLight"; }

        public override Type type => Entity.Type.InteriorLight;

        public InteriorLight(InteriorLightDefinition interiorLightDefinition) : base(interiorLightDefinition) { }
    }
}