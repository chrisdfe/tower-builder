using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Validators;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public interface IEntityGroupStateSlice
    {
        public ListWrapper<EntityGroup> entityGroupList { get; }

        public void Add(ListWrapper<EntityGroup> newItemsList);
        public void Add(EntityGroup item);
        public void Remove(ListWrapper<EntityGroup> removedItemsList);
        public void Remove(EntityGroup item);
        // public void Build(EntityGroup item);

        public IQueries entityQueries { get; }
        public IEvents entityEvents { get; }

        public interface IQueries
        {
            // public ListWrapper<EntityGroup> FindEntitiesAtCell(CellCoordinates cellCoordinates);
        }

        public interface IEvents
        {
            // public ListEvent<EntityGroup> onEntitiesAdded { get; set; }
            // public ListEvent<EntityGroup> onEntitiesRemoved { get; set; }
            // public ListEvent<EntityGroup> onEntitiesBuilt { get; set; }
        }
    }

    public class EntityGroupStateSlice<EntityGroupType, EventsType> : StateSlice, IEntityGroupStateSlice
        where EventsType : EntityGroupStateSlice<EntityGroupType, EventsType>.Events, new()
        where EntityGroupType : EntityGroup
    {
        public class Events : IEntityGroupStateSlice.IEvents
        {
            public ListEvent<EntityGroupType> onItemsAdded { get; set; }
            public ListEvent<Entity> onEntitiesAdded { get; set; }

            public ListEvent<EntityGroupType> onItemsRemoved { get; set; }
            public ListEvent<Entity> onEntitiesRemoved { get; set; }

            public ListEvent<EntityGroupType> onItemsBuilt { get; set; }
            public ListEvent<Entity> onEntitiesBuilt { get; set; }

            public ListEvent<EntityGroupType> onListUpdated { get; set; }
        }

        public ListWrapper<EntityGroupType> list { get; }
        public ListWrapper<EntityGroup> entityGroupList => list.ConvertAll<EntityGroup>();

        public EventsType events { get; }
        public IEntityGroupStateSlice.IEvents entityEvents => events as IEntityGroupStateSlice.IEvents;

        public Queries queries { get; protected set; }
        public IEntityGroupStateSlice.IQueries entityQueries => queries as IEntityGroupStateSlice.IQueries;

        public EntityGroupStateSlice(AppState appState) : base(appState)
        {
            list = new ListWrapper<EntityGroupType>();
            events = new EventsType();
            queries = new Queries(appState, this);
        }

        public void Add(ListWrapper<EntityGroupType> newItemsList)
        {
            list.Add(newItemsList);

            events.onItemsAdded?.Invoke(newItemsList);
            events.onEntitiesAdded?.Invoke(newItemsList.ConvertAll<Entity>());

            events.onListUpdated?.Invoke(list);
        }

        public void Add(ListWrapper<EntityGroup> newItemsList)
        {
            Add(newItemsList.ConvertAll<EntityGroupType>());
        }

        public void Add(EntityGroupType entityGroup)
        {
            ListWrapper<EntityGroupType> newItemsList = new ListWrapper<EntityGroupType>();
            newItemsList.Add(entityGroup);
            Add(newItemsList);
        }

        public void Add(EntityGroup entityGroup)
        {
            Add(entityGroup as EntityGroupType);
        }

        public void Add(ListWrapper<Entity> newItemsList)
        {
            Add(newItemsList as ListWrapper<EntityGroupType>);
        }

        public void Remove(ListWrapper<EntityGroupType> removedItemsList)
        {
            removedItemsList.items.ForEach((entityGroup) =>
            {
                // OnPreDestroy(entityGroup);

                // TODO - validation
                // TODO - add money back into wallet

                // entityGroup.OnDestroy();
            });

            list.Remove(removedItemsList);

            events.onItemsRemoved?.Invoke(removedItemsList);
            events.onEntitiesRemoved?.Invoke(removedItemsList.ConvertAll<Entity>());

            events.onListUpdated?.Invoke(list);
        }

        public void Remove(EntityGroupType item)
        {
            ListWrapper<EntityGroupType> removedItemsList = new ListWrapper<EntityGroupType>(item);
            Remove(removedItemsList);
        }

        public void Remove(ListWrapper<EntityGroup> entityGroupList)
        {
            Remove(entityGroupList as ListWrapper<EntityGroupType>);
        }

        public void Remove(EntityGroup entityGroup)
        {
            Remove(entityGroup as EntityGroupType);
        }

        public void Build(EntityGroupType entityGroup)
        {
            // if (!entity.validator.isValid)
            // {
            //     // TODO - these should be unique messages - right now they are not
            //     appState.Notifications.Add(entity.validator.errors);
            //     return;
            // }

            // 
            // appState.Wallet.SubtractBalance(entity.price);

            // OnPreBuild(entityGroup);
            // entity.OnBuild();

            ListWrapper<EntityGroupType> builtItemsList = new ListWrapper<EntityGroupType>();
            builtItemsList.Add(entityGroup);

            events.onItemsBuilt?.Invoke(builtItemsList);
            events.onEntitiesBuilt?.Invoke(builtItemsList.ConvertAll<Entity>());
        }

        public class Queries : IEntityGroupStateSlice.IQueries
        {
            protected AppState appState;
            protected EntityGroupStateSlice<EntityGroupType, EventsType> state;

            public Queries(AppState appState, EntityGroupStateSlice<EntityGroupType, EventsType> state)
            {
                this.appState = appState;
                this.state = state;
            }

            // public EntityGroupType FindEntityGroupTypeAtCell(CellCoordinates cellCoordinates) =>
            //     state.list.items
            //         .Find(entity => entity.cellCoordinatesList.Contains(cellCoordinates));


            // public Entity FindEntityAtCell(CellCoordinates cellCoordinates) =>
            //     FindEntityGroupTypeAtCell(cellCoordinates) as Entity;

            // public ListWrapper<EntityGroupType> FindEntityGroupTypesAtCell(CellCoordinates cellCoordinates) =>
            //     new ListWrapper<EntityGroupType>(
            //         state.list.items
            //             .FindAll(entity => entity.cellCoordinatesList.Contains(cellCoordinates))
            //     );

            // public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            //     FindEntityGroupTypesAtCell(cellCoordinates).ConvertAll<Entity>();

            // public ListWrapper<Entity> FindEntityByType(Type type) =>
            //     new ListWrapper<Entity>(
            //         state.entityList.items.FindAll(entity => entity.GetType() == type)
            //     );
        }
    }
}