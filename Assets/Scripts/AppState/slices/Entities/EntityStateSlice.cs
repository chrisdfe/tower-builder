using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public class EntityStateSlice : StateSlice
    {
        /*
            Events
        */
        public ListEvent<Entity> onItemsAdded { get; set; }
        public ListEvent<Entity> onItemsRemoved { get; set; }
        public ListEvent<Entity> onItemsBuilt { get; set; }

        public ListEvent<Entity> onListUpdated { get; set; }

        /*
            State
        */
        public ListWrapper<Entity> list { get; }

        public ListWrapper<Entity> entityList => list.ConvertAll<Entity>();

        public EntityStateSlice(AppState appState) : base(appState)
        {
            list = new ListWrapper<Entity>();
        }

        /* 
            Public API
        */
        public void Add(ListWrapper<Entity> newItemsList)
        {
            list.Add(newItemsList);

            onItemsAdded?.Invoke(newItemsList);
            onListUpdated?.Invoke(list);
        }

        public void Add(Entity entity)
        {
            ListWrapper<Entity> newItemsList = new ListWrapper<Entity>();
            newItemsList.Add(entity);
            Add(newItemsList);
        }

        public void Remove(ListWrapper<Entity> removedItemsList)
        {
            removedItemsList.items.ForEach((entity) =>
            {
                OnPreDestroy(entity);

                // TODO - validation
                // TODO - add money back into wallet

                entity.OnDestroy();
            });

            list.Remove(removedItemsList);

            onItemsRemoved?.Invoke(removedItemsList);

            onListUpdated?.Invoke(list);
        }

        public void Remove(Entity item)
        {
            ListWrapper<Entity> removedItemsList = new ListWrapper<Entity>(item);
            Remove(removedItemsList);
        }


        public void Build(Entity entity)
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

            ListWrapper<Entity> builtItemsList = new ListWrapper<Entity>();
            builtItemsList.Add(entity);

            onItemsBuilt?.Invoke(builtItemsList);
        }

        protected virtual void OnPreBuild(Entity entity) { }

        protected virtual void OnPreDestroy(Entity entity) { }

        /*
            Queries
        */
        public Entity FindEntityAtCell(CellCoordinates cellCoordinates) =>
            list.items
                .Find(entity => entity.cellCoordinatesList.Contains(cellCoordinates));

        public ListWrapper<Entity> FindEntitysAtCell(CellCoordinates cellCoordinates) =>
            new ListWrapper<Entity>(
                list.items
                    .FindAll(entity => entity.cellCoordinatesList.Contains(cellCoordinates))
            );

        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            FindEntitysAtCell(cellCoordinates).ConvertAll<Entity>();

        public ListWrapper<Entity> FindEntityByType(Type type) =>
            new ListWrapper<Entity>(
                entityList.items.FindAll(entity => entity.GetType() == type)
            );
    }
}