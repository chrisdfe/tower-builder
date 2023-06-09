using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups
{
    public class EntityGroupStateSlice : StateSlice
    {
        /*
            Events
        */
        public ListEvent<EntityGroup> onItemsAdded { get; set; }
        public ListEvent<EntityGroup> onItemsRemoved { get; set; }
        public ListEvent<EntityGroup> onItemsBuilt { get; set; }

        public ListEvent<EntityGroup> onListUpdated { get; set; }

        /*
            State
        */
        public ListWrapper<EntityGroup> list { get; }

        bool isListeningForEvents = true;

        public EntityGroupStateSlice(AppState appState) : base(appState)
        {
            list = new ListWrapper<EntityGroup>();
        }

        /*
            Lifecycle
        */
        public override void Setup()
        {
            base.Setup();

            appState.Entities.events.onEntitiesRemoved += OnEntitiesRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.events.onEntitiesRemoved -= OnEntitiesRemoved;
        }

        /* 
            Public API
        */
        public void Add(ListWrapper<EntityGroup> newItemsList)
        {
            list.Add(newItemsList);

            newItemsList.items.ForEach(entityGroup =>
            {
                foreach (var entry in entityGroup.groupedEntities)
                {
                    var (entityType, entitiesOfType) = entry;
                    appState.Entities.Add(entitiesOfType);
                }
            });

            onItemsAdded?.Invoke(newItemsList);
            onListUpdated?.Invoke(list);
        }

        public void Add(EntityGroup entityGroup)
        {
            ListWrapper<EntityGroup> newItemsList = new ListWrapper<EntityGroup>();
            newItemsList.Add(entityGroup);
            Add(newItemsList);
        }

        public void Remove(ListWrapper<EntityGroup> removedItemsList)
        {
            // Stop listening for events here to avoid an infinite loop
            isListeningForEvents = false;

            removedItemsList.items.ForEach((entityGroup) =>
            {
                // OnPreDestroy(entityGroup);

                // TODO - validation
                // TODO - add money back into wallet

                foreach (var entry in entityGroup.groupedEntities)
                {
                    var (entityType, entitiesOfType) = entry;
                    appState.Entities.Remove(entitiesOfType);
                }

                entityGroup.OnDestroy();
            });

            list.Remove(removedItemsList);

            onItemsRemoved?.Invoke(removedItemsList);
            onListUpdated?.Invoke(list);

            isListeningForEvents = true;
        }

        public void Remove(EntityGroup entityGroup)
        {
            ListWrapper<EntityGroup> removedItemsList = new ListWrapper<EntityGroup>(entityGroup);
            Remove(removedItemsList);
        }

        public void Build(EntityGroup entityGroup)
        {
            // if (!entity.validator.isValid)
            // {
            //     // TODO - these should be unique messages - right now they are not
            //     appState.Notifications.Add(entity.validator.errors);
            //     return;
            // }

            // 
            // appState.Wallet.SubtractBalance(entity.price);

            entityGroup.entities.items.ForEach(entity =>
            {
                appState.Entities.Build(entity);
            });

            entityGroup.OnBuild();

            ListWrapper<EntityGroup> builtItemsList = new ListWrapper<EntityGroup>();
            builtItemsList.Add(entityGroup);

            onItemsBuilt?.Invoke(builtItemsList);
        }

        /*
            Queries
        */
        public EntityGroup FindEntityGroupByEntity(Entity entity) =>
            list.Find(entityGroup => entityGroup.entities.items.Contains(entity));

        public ListWrapper<EntityGroup> FindEntityGroupsByEntities(ListWrapper<Entity> entities) =>
            list.FindAll(entityGroup =>
                entities.items.Find(entity => entityGroup.entities.Contains(entity)) != null
            );

        /*
            Internals
        */


        /*
            Event Handlers
        */
        void OnEntitiesRemoved(ListWrapper<Entity> removedEntities)
        {
            if (!isListeningForEvents) return;

            foreach (Entity removedEntity in removedEntities.items)
            {
                EntityGroup entityGroup = FindEntityGroupByEntity(removedEntity);

                if (entityGroup != null)
                {
                    entityGroup.Remove(removedEntity);

                    // TODO here - check if 
                }
            }
        }
    }
}