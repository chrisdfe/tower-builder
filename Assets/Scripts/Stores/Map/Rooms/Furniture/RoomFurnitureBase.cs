using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public abstract class RoomFurnitureBase
    {
        Room room;
        RoomFurnitureConfigBase config;

        public virtual RoomFurnitureCategory category { get { return RoomFurnitureCategory.None; } }

        public RoomFurnitureBase(Room room, RoomFurnitureConfigBase config)
        {
            this.room = room;
            this.config = config;
        }

        public abstract void Initialize();
        public abstract void OnDestroy();
    }
}