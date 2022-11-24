using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.FurnitureBuilders
{
    public class RoomFurnitureBuilderBase
    {
        protected Room room;

        public RoomFurnitureBuilderBase(Room room)
        {
            this.room = room;
        }

        public virtual FurnitureList BuildFurniture(bool isInBlueprintMode)
        {
            return new FurnitureList();
        }
    }
}


