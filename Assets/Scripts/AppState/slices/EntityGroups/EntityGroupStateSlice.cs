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
    public class EntityGroupStateSlice : ListStateSlice<EntityGroup>
    {
        /*
            Events
        */
        public ListEvent<EntityGroup> onItemsBuilt { get; set; }

        public delegate void EntityGroupEntitiesEvent(EntityGroup entityGroup, ListWrapper<Entity> entities);
        public EntityGroupEntitiesEvent onEntitiesAddedToEntityGroup { get; set; }
        public EntityGroupEntitiesEvent onEntitiesRemovedFromEntityGroup { get; set; }

        public delegate void EntityGroupEntityGroupsEvent(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroups);
        public EntityGroupEntityGroupsEvent onEntityGroupsAddedToEntityGroup { get; set; }
        public EntityGroupEntityGroupsEvent onEntityGroupsRemovedFromEntityGroup { get; set; }

        public ItemEvent<EntityGroup> onPositionUpdated { get; set; }

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

            appState.Entities.onItemsRemoved += OnEntitiesRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.onItemsRemoved -= OnEntitiesRemoved;
        }

        /* 
            Public Interface
        */
        public override void Add(ListWrapper<EntityGroup> newItemsList)
        {
            base.Add(newItemsList);

            newItemsList.items.ForEach(entityGroup =>
            {
                foreach (var entry in entityGroup.groupedEntities)
                {
                    var (entityType, entitiesOfType) = entry;
                    appState.Entities.Add(entitiesOfType);
                }
            });
        }

        public override void Remove(ListWrapper<EntityGroup> removedItemsList)
        {
            // Stop listening for events here to avoid an infinite loop,
            // since we're listening for onEntitiesRemoved
            isListeningForEvents = false;

            removedItemsList.items.ForEach((entityGroup) =>
            {
                // TODO - validation
                // TODO - add money back into wallet

                foreach (var entry in entityGroup.groupedEntities)
                {
                    var (entityType, entitiesOfType) = entry;
                    appState.Entities.Remove(entitiesOfType);
                }

                entityGroup.OnDestroy();
            });


            base.Remove(removedItemsList);

            isListeningForEvents = true;
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

        public void AddToEntityGroup(EntityGroup entityGroup, ListWrapper<Entity> entitiesToAdd)
        {
            entityGroup.Add(entitiesToAdd);

            onEntitiesAddedToEntityGroup?.Invoke(entityGroup, entitiesToAdd);
        }

        public void AddToEntityGroup(EntityGroup entityGroup, Entity entityToAdd)
        {
            ListWrapper<Entity> entitiesToAdd = new ListWrapper<Entity>(new List<Entity>() { entityToAdd });
            AddToEntityGroup(entityGroup, entitiesToAdd);
        }

        public void AddToEntityGroup(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroupsToAdd)
        {
            entityGroup.Add(entityGroupsToAdd);

            onEntityGroupsAddedToEntityGroup?.Invoke(entityGroup, entityGroupsToAdd);
        }

        public void AddToEntityGroup(EntityGroup entityGroup, EntityGroup entityGroupToAdd)
        {
            ListWrapper<EntityGroup> entityGroupsToAdd = new ListWrapper<EntityGroup>(new List<EntityGroup>() { entityGroupToAdd });
            AddToEntityGroup(entityGroup, entityGroupsToAdd);
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, ListWrapper<Entity> entitiesToRemove)
        {
            entityGroup.Remove(entitiesToRemove);

            onEntitiesAddedToEntityGroup?.Invoke(entityGroup, entitiesToRemove);

            RemoveEntityGroupIfEmpty(entityGroup);
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, Entity entityToRemove)
        {
            ListWrapper<Entity> entitiesToRemove = new ListWrapper<Entity>(new List<Entity>() { entityToRemove });

            RemoveFromEntityGroup(entityGroup, entitiesToRemove);

            RemoveEntityGroupIfEmpty(entityGroup);
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroupsToRemove)
        {
            entityGroup.Remove(entityGroupsToRemove);

            onEntityGroupsRemovedFromEntityGroup?.Invoke(entityGroup, entityGroupsToRemove);

            RemoveEntityGroupIfEmpty(entityGroup);
        }

        public void RemoveFromEntityGroup(EntityGroup entityGroup, EntityGroup entityGroupToRemove)
        {
            ListWrapper<EntityGroup> entitiesToRemove = new ListWrapper<EntityGroup>(new List<EntityGroup>() { entityGroupToRemove });

            RemoveFromEntityGroup(entityGroup, entitiesToRemove);

            RemoveEntityGroupIfEmpty(entityGroup);
        }

        /*
            Queries
        */
        public EntityGroup FindEntityGroupWithCellsOverlapping(CellCoordinatesList cellCoordinatesList) =>
            list.Find((entityGroup) => (
                entityGroup.entities.Find((entity) => (
                    entity.absoluteCellCoordinatesList.OverlapsWith(cellCoordinatesList)
                )) != null
            ));

        public EntityGroup FindEntityGroupAtCell(CellCoordinates cellCoordinates) =>
            list.Find(entityGroup => entityGroup.FindEntitiesAtCell(cellCoordinates) != null);

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

        /*
            Entities
        */
        void RemoveEntityGroupIfEmpty(EntityGroup entityGroup)
        {
            if (entityGroup.entities.Count == 0)
            {
                Remove(entityGroup);
            }
        }
    }
}