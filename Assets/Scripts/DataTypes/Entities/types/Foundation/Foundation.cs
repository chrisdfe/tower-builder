using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.GameWorld.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class Foundation : Entity
    {
        public override string idKey => "foundations";

        public Room room;

        public Foundation() { }
        public Foundation(FoundationDefinition foundationDefinition) : base(foundationDefinition) { }
    }
}