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
        public OfficeRoomBuilder() : base() { }

        public override Room Build(SelectionBox selectionBox)
        {
            return new Room();
        }
    }
}
