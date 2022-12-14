using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders
{
    public class RoomFurnitureBuilderBase
    {
        protected Room room;

        public RoomFurnitureBuilderBase(Room room)
        {
            this.room = room;
        }

        public virtual ListWrapper<Furniture> BuildFurniture(bool isInBlueprintMode)
        {
            return new ListWrapper<Furniture>();
        }
    }
}


