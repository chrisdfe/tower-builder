using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public abstract class EntityStateSliceBase : StateSlice
    {
        /*
            Events
        */
        public ListEvent<Entity> onItemsAdded { get; set; }
        public ListEvent<Entity> onItemsRemoved { get; set; }
        public ListEvent<Entity> onItemsBuilt { get; set; }

        public ListEvent<Entity> onListUpdated { get; set; }

        public ItemEvent<Entity> onEntityPositionUpdated { get; set; }

        /*
            State
        */
        public ListWrapper<Entity> list { get; }

        public EntityStateSliceBase(AppState appState) : base(appState)
        {
            list = new ListWrapper<Entity>();
        }

        /* 
            Public Interface
        */
        public void Add(ListWrapper<Entity> newItemsList)
        {
            list.Add(newItemsList);

            onItemsAdded?.Invoke(newItemsList);
            onListUpdated?.Invoke(list);
        }

        public void Add(Entity entity)
        {
            ListWrapper<Entity> newItemsList = new ListWrapper<Entity>(entity);
            Add(newItemsList);
        }

        public void Remove(ListWrapper<Entity> removedItemsList)
        {
            // TODO - validation
            removedItemsList.items.ForEach((entity) =>
            {
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
            entity.buildValidator.Validate(appState);

            if (entity.buildValidator.isValid)
            {
                // 
                appState.Wallet.SubtractBalance(entity.price);

                entity.OnBuild();

                ListWrapper<Entity> builtItemsList = new ListWrapper<Entity>(entity);
                onItemsBuilt?.Invoke(builtItemsList);
            }
            else
            {
                // TODO - these should be unique messages - right now they are not
                appState.Notifications.Add(entity.buildValidator.errors);
            }
        }

        public void UpdateEntityOffsetCoordinates(Entity entity, CellCoordinates offsetCoordinates)
        {
            entity.relativeOffsetCoordinates = offsetCoordinates;
            entity.buildValidator.Validate(appState);

            onEntityPositionUpdated?.Invoke(entity);
        }

        /*
            Queries
        */
        public Entity FindEntityAtCell(CellCoordinates cellCoordinates) =>
            list.items
                .Find(entity => appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).Contains(cellCoordinates));

        public ListWrapper<Entity> FindEntitysAtCell(CellCoordinates cellCoordinates) =>
            new ListWrapper<Entity>(
                list.items
                    .FindAll(entity => appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).Contains(cellCoordinates))
            );

        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            FindEntitysAtCell(cellCoordinates).ConvertAll<Entity>();

        public ListWrapper<Entity> FindEntityByType(Type type) =>
            new ListWrapper<Entity>(
                list.items.FindAll(entity => entity.GetType() == type)
            );
    }
}