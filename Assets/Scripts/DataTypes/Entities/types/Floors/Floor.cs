using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.GameWorld.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class Floor : Entity
    {
        public override string idKey { get => "floors"; }

        public Room room;

        public Floor(FloorDefinition floorDefinition) : base(floorDefinition) { }
    }
}