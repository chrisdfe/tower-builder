using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Behaviors
{
    public abstract class FurnitureBehaviorBase
    {
        public enum Key
        {
            None,
            Default,
            Cockpit,
            Bed,
            Engine,
            MoneyMachine
        };

        public Furniture furniture { get; private set; }

        public ResidentsList interactingResidentsList { get; private set; } = new ResidentsList();

        public virtual string title { get; }

        public virtual FurnitureBehaviorTag[] tags { get; }

        public abstract Key key { get; }

        public bool isActive { get { return interactingResidentsList.Count > 0; } }

        protected AppState appState;

        public FurnitureBehaviorBase(AppState appState, Furniture furniture)
        {
            this.appState = appState;
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

        public virtual void InteractTick(Resident resident) { }
    }
}