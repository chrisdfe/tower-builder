using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Behaviors
{
    public class FurnitureBehaviorList : ListWrapper<FurnitureBehaviorBase>
    {
        public FurnitureBehaviorList() : base() { }
        public FurnitureBehaviorList(FurnitureBehaviorBase furnitureBehavior) : base(furnitureBehavior) { }
        public FurnitureBehaviorList(List<FurnitureBehaviorBase> furnitureBehaviors) : base(furnitureBehaviors) { }
        public FurnitureBehaviorList(FurnitureBehaviorList furnitureBehaviorList) : base(furnitureBehaviorList) { }

        public FurnitureBehaviorBase FindByFurniture(Furniture furniture)
        {
            return items.Find(furnitureBehavior => furnitureBehavior.furniture == furniture);
        }

        public FurnitureBehaviorList FilterByType(FurnitureBehaviorBase.Key key)
        {
            return new FurnitureBehaviorList(
                items.FindAll(furnitureBehavior => furnitureBehavior.key == key)
            );
        }
    }
}