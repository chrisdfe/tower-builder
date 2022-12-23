using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Validators.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public abstract class FurnitureBehavior
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

        public ListWrapper<Resident> interactingResidentsList { get; private set; } = new ListWrapper<Resident>();

        public virtual string title { get; }

        public abstract Key key { get; }

        public bool isActive => interactingResidentsList.Count > 0;

        public FurnitureBehaviorValidator validator;
        protected virtual FurnitureBehaviorValidator createValidator() => new FurnitureBehaviorValidator(this);

        protected AppState appState;

        public FurnitureBehavior(AppState appState, Furniture furniture)
        {
            this.appState = appState;
            this.furniture = furniture;
            this.validator = createValidator();
        }

        public void StartInteraction(Resident resident)
        {
            validator.Validate(appState);

            if (validator.isValid)
            {
                interactingResidentsList.Add(resident);
                OnInteractStart(resident);
            }
            else
            {
                appState.Notifications.Add(
                    new ListWrapper<Notification>(
                        validator.errors.items.Select(error => new Notification(error.message)).ToList()
                    )
                );
            }
        }

        public void EndInteraction(Resident resident)
        {
            interactingResidentsList.Remove(resident);
            OnInteractEnd(resident);
        }

        public void InteractTick(Resident resident)
        {
            OnInteractTick(resident);
        }

        protected virtual void OnInteractStart(Resident resident) { }
        protected virtual void OnInteractEnd(Resident resident) { }
        protected virtual void OnInteractTick(Resident resident) { }
    }
}