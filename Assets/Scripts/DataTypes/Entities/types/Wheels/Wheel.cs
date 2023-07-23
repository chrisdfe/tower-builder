using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Wheels
{
    public class Wheel : Entity
    {
        public override string idKey { get => "wheels"; }

        public Wheel() : base() { }
        public Wheel(Input input) : base(input) { }
        public Wheel(WheelDefinition wheelDefinition) : base(wheelDefinition) { }
    }
}