using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
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

        public FurnitureBehaviorList FindByRoom(Room room)
        {
            FurnitureBehaviorList f = new FurnitureBehaviorList(
                items.FindAll(furnitureBehavior => furnitureBehavior.furniture.room == room)
            );

            return f;
        }

        public FurnitureBehaviorList FilterByType(FurnitureBehaviorBase.Key key)
        {
            return new FurnitureBehaviorList(
                items.FindAll(furnitureBehavior => furnitureBehavior.key == key)
            );
        }
    }
}