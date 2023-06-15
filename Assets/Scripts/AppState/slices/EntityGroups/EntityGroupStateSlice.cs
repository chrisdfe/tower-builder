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

        public ItemEvent<EntityGroup> onPositionUpdated { get; set; }

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

            appState.Entities.onEntitiesRemoved += OnEntitiesRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.onEntitiesRemoved -= OnEntitiesRemoved;
        }

        /* 
            Public Interface
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
            entityGroup.Validate(appState);

            if (!entityGroup.isValid)
            {
                appState.Notifications.Add(entityGroup.validationErrors);
                return;
            }

            appState.Wallet.SubtractBalance(entityGroup.price);

            // TODO - use groupedEntities for this instead
            entityGroup.entities.items.ForEach(entity =>
            {
                appState.Entities.Build(entity);
            });

            entityGroup.OnBuild();

            ListWrapper<EntityGroup> builtItemsList = new ListWrapper<EntityGroup>();
            builtItemsList.Add(entityGroup);

            onItemsBuilt?.Invoke(builtItemsList);
        }

        public void UpdateOffsetCoordinates(EntityGroup entityGroup, CellCoordinates newOffsetCoordinates)
        {
            entityGroup.offsetCoordinates = newOffsetCoordinates;
            entityGroup.Validate(appState);

            onPositionUpdated?.Invoke(entityGroup);
        }

        public void AddToEntityGroup(EntityGroup entityGroup, Entity entityToAdd)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void AddToEntityGroup(EntityGroup entityGroup, ListWrapper<Entity> entitiesToAdd)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void AddToEntityGroup(EntityGroup entityGroup, EntityGroup entityGroupToAdd)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void AddToEntityGroup(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroupsToAdd)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, Entity entityToRemove)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, ListWrapper<Entity> entitiesToRemove)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, EntityGroup entityGroupToRemove)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroupsToRemove)
        {
            throw new System.NotImplementedException("I haven't imlemented this yet");
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

                    // TODO here - check if there are any entities left in entityGroup
                    // if not then remove the entityGroup as well
                }
            }
        }
    }
}