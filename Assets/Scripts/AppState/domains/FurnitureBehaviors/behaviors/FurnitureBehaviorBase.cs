using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.State.Rooms.Furnitures
{
    public abstract class FurnitureBehaviorBase
    {
        Furniture roomFurniture;
        Resident[] usageSlots;

        public FurnitureBehaviorBase(Furniture roomFurniture)
        {
            this.roomFurniture = roomFurniture;
        }

        public abstract void OnInteractStart(Resident resident);
        public abstract void OnInteractEnd(Resident resident);
        public abstract bool CanInteractWith(Resident resident);
    }
}