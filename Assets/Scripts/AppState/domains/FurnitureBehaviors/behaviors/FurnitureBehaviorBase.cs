using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furniture;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public abstract class FurnitureBehaviorBase
    {
        FurnitureBase roomFurniture;
        Resident[] usageSlots;

        public FurnitureBehaviorBase(FurnitureBase roomFurniture)
        {
            this.roomFurniture = roomFurniture;
        }

        public abstract void OnInteractStart(Resident resident);
        public abstract void OnInteractEnd(Resident resident);
        public abstract bool CanInteractWith(Resident resident);
    }
}