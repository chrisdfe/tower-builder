using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Residents;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public abstract class RoomFurnitureBehaviorBase
    {
        RoomFurnitureBase roomFurniture;

        public RoomFurnitureBehaviorBase(RoomFurnitureBase roomFurniture)
        {
            this.roomFurniture = roomFurniture;
        }

        public abstract void OnInteractStart(Resident resident);
        public abstract void OnInteractEnd(Resident resident);
    }
}