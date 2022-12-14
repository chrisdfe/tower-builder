using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public interface IEntityStateSlice
    {
        public void Add(ListWrapper<Entity> newItemsList);
        public void Add(Entity item);
        public void Remove(ListWrapper<Entity> removedItemsList);
        public void Remove(Entity item);
        public void Build(Entity item);
    }

    public class EntityStateSlice<EntityType, EventsType> : StateSlice, IEntityStateSlice
        where EventsType : EntityStateSlice<EntityType, EventsType>.Events, new()
        where EntityType : Entity
    {
        public class Events
        {
            public ListEvent<EntityType> onItemsAdded { get; set; }
            public ListEvent<EntityType> onItemsRemoved { get; set; }
            public ListEvent<EntityType> onListUpdated { get; set; }
            public ListEvent<EntityType> onItemsBuilt { get; set; }
        }

        public ListWrapper<EntityType> list { get; }
        public EventsType events { get; }
        public Queries queries { get; }

        public EntityStateSlice(AppState appState) : base(appState)
        {
            list = new ListWrapper<EntityType>();
            events = new EventsType();
            queries = new Queries(appState, this);
        }

        public void Add(ListWrapper<EntityType> newItemsList)
        {
            list.Add(newItemsList);
            events.onItemsAdded?.Invoke(newItemsList);
            events.onListUpdated?.Invoke(list);
        }

        public void Add(EntityType entity)
        {
            ListWrapper<EntityType> newItemsList = new ListWrapper<EntityType>();
            newItemsList.Add(entity);
            Add(newItemsList);
        }

        public void Add(Entity entity)
        {
            // entity.validator.Validate(appState);
            Add(entity as EntityType);
        }

        public void Add(ListWrapper<Entity> newItemsList)
        {
            Add(newItemsList as ListWrapper<EntityType>);
        }

        public void Remove(ListWrapper<EntityType> removedItemsList)
        {
            removedItemsList.items.ForEach((entity) =>
            {
                OnPreDestroy(entity);
                // TODO - validation
                // TODO - add money back into wallet

                entity.OnDestroy();
            });

            list.Remove(removedItemsList);

            events.onItemsRemoved?.Invoke(removedItemsList);
            events.onListUpdated?.Invoke(list);
        }

        public void Remove(EntityType item)
        {
            ListWrapper<EntityType> removedItemsList = new ListWrapper<EntityType>();
            removedItemsList.Add(item);
            Remove(removedItemsList);
        }

        public void Remove(ListWrapper<Entity> entityList)
        {
            Remove(entityList as ListWrapper<EntityType>);
        }

        public void Remove(Entity entity)
        {
            Remove(entity as EntityType);
        }

        public void Build(EntityType entity)
        {
            // TODO - don't do this here
            // entity.validator.Validate(appState);

            if (!entity.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (EntityValidationError validationError in entity.validator.errors.items)
                {
                    appState.Notifications.Add(new Notification(validationError.message));
                }
                return;
            }

            // 
            appState.Wallet.SubtractBalance(entity.price);

            OnPreBuild(entity);
            entity.OnBuild();

            ListWrapper<EntityType> listWrapper = new ListWrapper<EntityType>();
            listWrapper.Add(entity);
            events.onItemsBuilt?.Invoke(listWrapper);
        }

        public void Build(Entity entity)
        {
            Build(entity as EntityType);
        }

        protected virtual void OnPreBuild(EntityType entity) { }

        protected virtual void OnPreDestroy(EntityType entity) { }

        public class Queries
        {
            protected AppState appState;
            protected EntityStateSlice<EntityType, EventsType> state;

            public Queries(AppState appState, EntityStateSlice<EntityType, EventsType> state)
            {
                this.appState = appState;
                this.state = state;
            }

            public EntityType FindEntityTypeAtCell(CellCoordinates cellCoordinates) =>
                state.list.items.Find(entity => entity.cellCoordinatesList.Contains(cellCoordinates));
        }
    }
}