using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public abstract class FurnitureBehaviorBase
    {
        public Furniture furniture { get; private set; }

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
            Debug.Log("Interact start");
            interactingResidentsList.Add(resident);
        }

        public virtual void InteractEnd(Resident resident)
        {
            Debug.Log("Interact end");
            interactingResidentsList.Remove(resident);
        }

        public virtual void InteractTick(AppState appState) { }
    }
}