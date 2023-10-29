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

            appState.Entities.onItemsAdded += OnEntitiesAdded;
            appState.Entities.onItemsRemoved += OnEntitiesRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.onItemsAdded += OnEntitiesAdded;
            appState.Entities.onItemsRemoved -= OnEntitiesRemoved;
        }

        /* 
            Public Interface
        */
        public override void Add(ListWrapper<EntityGroup> newItemsList)
        {
            base.Add(newItemsList);

            newItemsList.items.ForEach((entityGroup) =>
            {
                Add(entityGroup);
            });
        }

        public override void Add(EntityGroup newEntityGroup)
        {
            // TODO - this null check should probably be in ListWrapper.Add instead/as well
            if (newEntityGroup == null) return;

            base.Add(newEntityGroup);
        }

        public override void Remove(ListWrapper<EntityGroup> removedItemsList)
        {
            // Stop listening for events here to avoid an infinite loop,
            // since we're listening for onEntitiesRemoved
            isListeningForEvents = false;

            removedItemsList.items.ForEach((entityGroup) =>
            {
                Remove(entityGroup);
            });

            base.Remove(removedItemsList);

            isListeningForEvents = true;
        }

        public override void Remove(EntityGroup entityGroupToRemove)
        {
            if (entityGroupToRemove == null) return;

            isListeningForEvents = false;

            entityGroupToRemove.destroyValidator.Validate(appState);

            if (entityGroupToRemove.destroyValidator.isValid)
            {
                // TODO - add money back into wallet
                entityGroupToRemove.OnDestroy();
                base.Remove(entityGroupToRemove);
            }
            else
            {
                appState.Notifications.Add(entityGroupToRemove.destroyValidator.errors);
            }

            isListeningForEvents = true;
        }

        public void Build(EntityGroup entityGroup)
        {
            entityGroup.buildValidator.Validate(appState);

            if (entityGroup.buildValidator.isValid)
            {
                appState.Wallet.SubtractBalance(entityGroup.price);

                entityGroup.OnBuild();

                ListWrapper<EntityGroup> builtItemsList = new ListWrapper<EntityGroup>();
                builtItemsList.Add(entityGroup);

                onItemsBuilt?.Invoke(builtItemsList);
            }
            else
            {
                appState.Notifications.Add(entityGroup.buildValidator.errors);
            }
        }

        public void Build(ListWrapper<EntityGroup> entityGroups)
        {
            foreach (EntityGroup entityGroup in entityGroups.items)
            {
                Build(entityGroup);
            }
        }

        public void UpdateOffsetCoordinates(EntityGroup entityGroup, CellCoordinates newOffsetCoordinates)
        {
            entityGroup.relativeOffsetCoordinates = newOffsetCoordinates;
            entityGroup.buildValidator.Validate(appState);

            onPositionUpdated?.Invoke(entityGroup);
        }

        public void AddChild(EntityGroup entityGroup, Entity entityToAdd)
        {
            ListWrapper<Entity> entitiesToAdd = new ListWrapper<Entity>(new List<Entity>() { entityToAdd });
            AddChildren(entityGroup, entitiesToAdd);
        }

        public void AddChild(EntityGroup entityGroup, EntityGroup entityGroupToAdd)
        {
            ListWrapper<EntityGroup> entityGroupsToAdd = new ListWrapper<EntityGroup>(new List<EntityGroup>() { entityGroupToAdd });
            AddChildren(entityGroup, entityGroupsToAdd);
        }

        public void AddChildren(EntityGroup entityGroup, ListWrapper<Entity> entitiesToAdd)
        {
            entityGroup.Add(entitiesToAdd);
            onEntitiesAddedToEntityGroup?.Invoke(entityGroup, entitiesToAdd);
        }

        public void AddChildren(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroupsToAdd)
        {
            entityGroup.Add(entityGroupsToAdd);
            onEntityGroupsAddedToEntityGroup?.Invoke(entityGroup, entityGroupsToAdd);
        }

        public void RemoveChild(EntityGroup entityGroup, Entity entityToRemove)
        {
            ListWrapper<Entity> entitiesToRemove = new ListWrapper<Entity>(new List<Entity>() { entityToRemove });
            RemoveChildren(entityGroup, entitiesToRemove);
        }

        public void RemoveChild(EntityGroup entityGroup, EntityGroup entityGroupToRemove)
        {
            ListWrapper<EntityGroup> entitiesToRemove = new ListWrapper<EntityGroup>(new List<EntityGroup>() { entityGroupToRemove });
            RemoveChildren(entityGroup, entitiesToRemove);
        }

        public void RemoveChildren(EntityGroup entityGroup, ListWrapper<EntityGroup> entityGroupsToRemove)
        {
            entityGroup.Remove(entityGroupsToRemove);

            onEntityGroupsRemovedFromEntityGroup?.Invoke(entityGroup, entityGroupsToRemove);

            RemoveEntityGroupIfEmpty(entityGroup);
        }

        public void RemoveChildren(EntityGroup entityGroup, ListWrapper<Entity> entitiesToRemove)
        {
            entityGroup.Remove(entitiesToRemove);
            onEntitiesAddedToEntityGroup?.Invoke(entityGroup, entitiesToRemove);

            RemoveEntityGroupIfEmpty(entityGroup);
        }

        /*
            Queries
        */
        public EntityGroup FindEntityGroupWithCellsOverlapping(CellCoordinatesList cellCoordinatesList) =>
            list.Find((entityGroup) =>
                entityGroup.GetDescendantEntities().Find((entity) => (
                    appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).OverlapsWith(cellCoordinatesList)
                )) != null
            );

        public EntityGroup FindEntityGroupAtCell(CellCoordinates cellCoordinates) =>
            list.Find((entityGroup) =>
                entityGroup.GetDescendantEntities().Find((entity) => (
                    appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).Contains(cellCoordinates)
                )) != null
            );

        public EntityGroup FindEntityParent(Entity entity) =>
            list.Find(entityGroup => entityGroup.childEntities.items.Contains(entity));

        public EntityGroup FindEntityGroupParent(EntityGroup targetEntityGroup) =>
            list.Find(entityGroup => entityGroup.childEntityGroups.Contains(targetEntityGroup));

        /*
            Event Handlers
        */
        void OnEntitiesAdded(ListWrapper<Entity> addedEntities)
        {
            if (!isListeningForEvents) return;
        }

        void OnEntitiesRemoved(ListWrapper<Entity> removedEntities)
        {
            if (!isListeningForEvents) return;

            foreach (Entity removedEntity in removedEntities.items)
            {
                EntityGroup entityGroup = FindEntityParent(removedEntity);

                if (entityGroup != null)
                {
                    entityGroup.Remove(removedEntity);
                    RemoveEntityGroupIfEmpty(entityGroup);
                }
            }
        }

        /*
            Internals
        */
        void RemoveEntityGroupIfEmpty(EntityGroup entityGroup)
        {
            if (entityGroup.GetDescendantEntities().Count == 0)
            {
                Remove(entityGroup);
            }
        }
    }
}