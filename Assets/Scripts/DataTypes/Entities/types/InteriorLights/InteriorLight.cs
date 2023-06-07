using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.InteriorLights
{
    public class InteriorLight : Entity
    {
        public override string idKey { get => "interiorLight"; }

        public InteriorLight(InteriorLightDefinition interiorLightDefinition) : base(interiorLightDefinition) { }
    }
}