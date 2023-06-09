using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class OfficeRoomBuilder : RoomBuilderBase
    {
        public OfficeRoomBuilder(EntityGroupDefinition definition) : base(definition) { }

        public override EntityGroup Build(SelectionBox selectionBox)
        {
            Room room = new Room();

            // TODO

            return room;
        }
    }
}
