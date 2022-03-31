using System.Collections;
using System.Collections.Generic;


using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Furniture
{
    public abstract class RoomFurnitureBase
    {
        Room room;
        RoomFurnitureAttributesBase config;

        public virtual RoomFurnitureCategory category { get { return RoomFurnitureCategory.None; } }

        public RoomFurnitureBase(Room room, RoomFurnitureAttributesBase config)
        {
            this.room = room;
            this.config = config;
        }

        public abstract void Initialize();
        public abstract void OnDestroy();
    }
}