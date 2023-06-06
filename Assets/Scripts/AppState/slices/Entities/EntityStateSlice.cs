using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public interface IEntityStateSlice : ISetupable
    {
        public ListWrapper<Entity> entityList { get; }

        public void Add(ListWrapper<Entity> newItemsList);
        public void Add(Entity item);
        public void Remove(ListWrapper<Entity> removedItemsList);
        public void Remove(Entity item);
        public void Build(Entity item);

        public IQueries entityQueries { get; }
        public IEvents entityEvents { get; }

        public interface IQueries
        {
            public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates);
        }

        public interface IEvents
        {
            public ListEvent<Entity> onEntitiesAdded { get; set; }
            public ListEvent<Entity> onEntitiesRemoved { get; set; }
            public ListEvent<Entity> onEntitiesBuilt { get; set; }
        }
    }

    public class EntityStateSlice<EntityType, EventsType> : StateSlice, IEntityStateSlice
        where EventsType : EntityStateSlice<EntityType, EventsType>.Events, new()
        where EntityType : Entity
    {
        public class Events : IEntityStateSlice.IEvents
        {
            public ListEvent<EntityType> onItemsAdded { get; set; }
            public ListEvent<Entity> onEntitiesAdded { get; set; }

            public ListEvent<EntityType> onItemsRemoved { get; set; }
            public ListEvent<Entity> onEntitiesRemoved { get; set; }

            public ListEvent<EntityType> onItemsBuilt { get; set; }
            public ListEvent<Entity> onEntitiesBuilt { get; set; }

            public ListEvent<EntityType> onListUpdated { get; set; }
        }

        public ListWrapper<EntityType> list { get; }

        public EventsType events { get; }
        public IEntityStateSlice.IEvents entityEvents => events as IEntityStateSlice.IEvents;

        public Queries queries { get; protected set; }
        public IEntityStateSlice.IQueries entityQueries => queries as IEntityStateSlice.IQueries;

        public ListWrapper<Entity> entityList => list.ConvertAll<Entity>();

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
            events.onEntitiesAdded?.Invoke(newItemsList.ConvertAll<Entity>());

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
            events.onEntitiesRemoved?.Invoke(removedItemsList.ConvertAll<Entity>());

            events.onListUpdated?.Invoke(list);
        }

        public void Remove(EntityType item)
        {
            ListWrapper<EntityType> removedItemsList = new ListWrapper<EntityType>(item);
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
            if (!entity.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                appState.Notifications.Add(entity.validator.errors);
                return;
            }

            // 
            appState.Wallet.SubtractBalance(entity.price);

            OnPreBuild(entity);
            entity.OnBuild();

            ListWrapper<EntityType> builtItemsList = new ListWrapper<EntityType>();
            builtItemsList.Add(entity);

            events.onItemsBuilt?.Invoke(builtItemsList);
            events.onEntitiesBuilt?.Invoke(builtItemsList.ConvertAll<Entity>());
        }

        public void Build(Entity entity)
        {
            Build(entity as EntityType);
        }

        protected virtual void OnPreBuild(EntityType entity) { }

        protected virtual void OnPreDestroy(EntityType entity) { }

        public class Queries : IEntityStateSlice.IQueries
        {
            protected AppState appState;
            protected EntityStateSlice<EntityType, EventsType> state;

            public Queries(AppState appState, EntityStateSlice<EntityType, EventsType> state)
            {
                this.appState = appState;
                this.state = state;
            }

            public EntityType FindEntityTypeAtCell(CellCoordinates cellCoordinates) =>
                state.list.items
                    .Find(entity => entity.cellCoordinatesList.Contains(cellCoordinates));


            public Entity FindEntityAtCell(CellCoordinates cellCoordinates) =>
                FindEntityTypeAtCell(cellCoordinates) as Entity;

            public ListWrapper<EntityType> FindEntityTypesAtCell(CellCoordinates cellCoordinates) =>
                new ListWrapper<EntityType>(
                    state.list.items
                        .FindAll(entity => entity.cellCoordinatesList.Contains(cellCoordinates))
                );

            public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
                FindEntityTypesAtCell(cellCoordinates).ConvertAll<Entity>();

            public ListWrapper<Entity> FindEntityByType(Type type) =>
                new ListWrapper<Entity>(
                    state.entityList.items.FindAll(entity => entity.GetType() == type)
                );
        }
    }
}