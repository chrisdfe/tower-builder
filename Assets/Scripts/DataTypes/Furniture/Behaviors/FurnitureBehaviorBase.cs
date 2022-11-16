using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public abstract class FurnitureBehaviorBase
    {
        protected Furniture furniture;

        public ResidentsList interactingResidentsList { get; private set; } = new ResidentsList();

        public virtual string title { get; }

        public virtual FurnitureBehaviorTag[] tags { get; }

        public bool isActive { get { return interactingResidentsList.Count > 0; } }

        public FurnitureBehaviorBase(Furniture furniture)
        {
            this.furniture = furniture;
        }

        public virtual void InteractStart(Resident resident)
        {
            interactingResidentsList.Add(resident);
        }

        public virtual void InteractEnd(Resident resident)
        {
            interactingResidentsList.Remove(resident);
        }

        public virtual void InteractTick() { }
    }
}