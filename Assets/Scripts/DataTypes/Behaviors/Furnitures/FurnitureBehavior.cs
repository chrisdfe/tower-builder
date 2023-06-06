using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Notifications;
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

        public Validator<FurnitureBehavior> validator;
        protected virtual Validator<FurnitureBehavior> createValidator() => new Validator<FurnitureBehavior>(this);

        protected AppState appState;

        public FurnitureBehavior(AppState appState, Furniture furniture)
        {
            this.appState = appState;
            this.furniture = furniture;
            this.validator = createValidator();
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public bool StartInteraction(Resident resident)
        {
            validator.Validate(appState);

            if (validator.isValid)
            {
                interactingResidentsList.Add(resident);
                OnInteractStart(resident);
            }
            else
            {
                AddValidationErrorNotifications();
                return false;
            }

            return true;
        }

        public bool EndInteraction(Resident resident)
        {
            interactingResidentsList.Remove(resident);
            OnInteractEnd(resident);
            return true;
        }

        // TODO - validate every tick and return false if it is not valied
        public bool InteractTick(Resident resident)
        {
            validator.Validate(appState);

            if (validator.isValid)
            {
                OnInteractTick(resident);
            }
            else
            {
                AddValidationErrorNotifications();
                return false;
            }

            return true;
        }

        protected virtual void OnInteractStart(Resident resident) { }
        protected virtual void OnInteractEnd(Resident resident) { }
        protected virtual void OnInteractTick(Resident resident) { }

        /*
            Internals
        */
        void AddValidationErrorNotifications() =>
            appState.Notifications.Add(validator.errors);
    }
}