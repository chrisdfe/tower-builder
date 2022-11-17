using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class FurnitureBehaviorList : ListWrapper<FurnitureBehaviorBase>
    {
        public FurnitureBehaviorBase FindByFurniture(Furniture furniture)
        {
            return items.Find(furnitureBehavior => furnitureBehavior.furniture == furniture);
        }
    }
}