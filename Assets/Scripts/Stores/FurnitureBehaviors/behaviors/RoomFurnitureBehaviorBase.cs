using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Residents;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Furniture
{
    public abstract class RoomFurnitureBehaviorBase
    {
        RoomFurnitureBase roomFurniture;
        Resident[] usageSlots;

        public RoomFurnitureBehaviorBase(RoomFurnitureBase roomFurniture)
        {
            this.roomFurniture = roomFurniture;
        }

        public abstract void OnInteractStart(Resident resident);
        public abstract void OnInteractEnd(Resident resident);
        public abstract bool CanInteractWith(Resident resident);
    }
}