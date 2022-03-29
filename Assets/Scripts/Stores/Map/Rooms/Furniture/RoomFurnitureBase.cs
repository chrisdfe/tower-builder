using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public abstract class RoomFurnitureBase
    {
        Room room;
        RoomFurnitureOwnability ownability;
        float roomBeautyScore;
        float price;

        public virtual RoomFurnitureCategory category { get { return RoomFurnitureCategory.None; } }

        public RoomFurnitureBase(Room room, RoomConfigBase config)
        {
            this.room = room;

            this.ownability = config.ownability;
            this.roomBeautyScore = config.roomBeautyScore;
        }

        public abstract void Initialize();
        public abstract void OnDestroy();
    }
}