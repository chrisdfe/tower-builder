using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Attributes.Vehicles
{
    public class VehicleAttribute : Attribute<VehicleAttribute.Key>
    {
        public enum Key
        {
            CurrentSpeed,
            Fuel,
            Weight,
            EnginePower,
            TargetSpeed,
            MaxSpeed,
            IsPiloted,
        }

        public VehicleAttribute(Key key) : base(key) { }
        public VehicleAttribute(Key key, float initialValue) : base(key, initialValue) { }
        public VehicleAttribute(Key key, float initialValue, float min, float max) : base(key, initialValue, min, max) { }

        public VehicleAttribute(Key key, List<AttributeModifier> initialStaticModifiers, List<AttributeModifier> initialTickModifiers)
            : base(key, initialTickModifiers, initialTickModifiers) { }
    }
}