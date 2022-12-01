using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Vehicles
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
            MaxSpeed
        }

        public VehicleAttribute(Key key) : base(key) { }
        public VehicleAttribute(Key key, float initialValue) : base(key, initialValue) { }
        public VehicleAttribute(Key key, float initialValue, float min, float max) : base(key, initialValue, min, max) { }

        public VehicleAttribute(Key key, List<VehicleAttribute.Modifier> initialStaticModifiers, List<VehicleAttribute.Modifier> initialTickModifiers)
            : base(key, initialTickModifiers, initialTickModifiers) { }
    }
}