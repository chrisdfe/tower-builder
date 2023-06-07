using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomBuilderBase : EntityGroupBuilderBase<Room>
    {
        Room currentRoom;

        public RoomBuilderBase() : base() { }

        public override Room Build(SelectionBox selectionBox)
        {
            currentRoom = new Room();

            BuildFoundation(selectionBox);
            BuildFurniture(selectionBox);
            BuildWindows(selectionBox);
            BuildTransportationItems(selectionBox);

            return currentRoom;
        }

        protected virtual void BuildFoundation(SelectionBox selectionBox) { }
        protected virtual void BuildFurniture(SelectionBox selectionBox) { }
        protected virtual void BuildWindows(SelectionBox selectionBox) { }
        protected virtual void BuildTransportationItems(SelectionBox selectionBox) { }
    }
}


