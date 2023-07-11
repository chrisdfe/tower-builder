using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable
    {
        public ListWrapper<Entity> childEntities { get; } = new ListWrapper<Entity>();
        public ListWrapper<EntityGroup> childEntityGroups { get; } = new ListWrapper<EntityGroup>();

        public int id { get; }

        public bool isInBlueprintMode { get; private set; } = false;

        public EntityGroupBuildValidator buildValidator;
        public EntityGroupDestroyValidator destroyValidator;

        public virtual string typeLabel => "EntityGroup";

        public int price =>
            GetDescendantEntities().items.Aggregate(0, ((acc, entity) => acc + entity.price));

        public CellCoordinates relativeOffsetCoordinates { get; set; } = CellCoordinates.zero;

        public override string ToString() => $"{typeLabel} #{id}";

        public EntityGroup()
        {
            id = UIDGenerator.Generate(typeLabel);

            buildValidator = new EntityGroupBuildValidator(this);
            destroyValidator = new EntityGroupDestroyValidator(this);
        }

        public EntityGroup(EntityGroupDefinition definition) : this() { }

        /*
            Lifecycle
        */
        public virtual void Setup() { }

        public virtual void Teardown() { }

        public virtual void OnBuild()
        {
            SetBlueprintMode(false);
        }

        public virtual void OnDestroy() { }

        /*
            Public Interface
        */
        public void Add(Entity entity)
        {
            childEntities.Add(entity);
            entity.isInBlueprintMode = isInBlueprintMode;
        }

        public void Add(ListWrapper<Entity> entitiesList)
        {
            childEntities.Add(entitiesList);
            entitiesList.ForEach(entity =>
            {
                entity.isInBlueprintMode = isInBlueprintMode;
            });
        }

        public void Add(EntityGroup entityGroup)
        {
            childEntityGroups.Add(entityGroup);
            entityGroup.SetBlueprintMode(isInBlueprintMode);
        }

        public void Add(ListWrapper<EntityGroup> entityGroupList)
        {
            childEntityGroups.Add(entityGroupList);
            entityGroupList.ForEach((entityGroup) =>
            {
                entityGroup.SetBlueprintMode(isInBlueprintMode);
            });
        }

        public void Remove(Entity entity)
        {
            childEntities.Remove(entity);
        }

        public void Remove(ListWrapper<Entity> entitiesList)
        {
            childEntities.Remove(entitiesList);
        }

        public void Remove(EntityGroup entityGroup)
        {
            childEntityGroups.Remove(entityGroup);
        }

        public void Remove(ListWrapper<EntityGroup> entityGroups)
        {
            childEntityGroups.Remove(entityGroups);
        }

        public void SetBlueprintMode(bool isInBlueprintMode)
        {
            this.isInBlueprintMode = isInBlueprintMode;
        }

        public void UpdateOffsetCoordinatesAndChildren(CellCoordinates relativeOffsetCoordinatesToAdd)
        {
            // this.offsetCoordinates = offsetCoordinates;
            // UpdateChildEntityOffsets(this, offsetCoordinates);
            // UpdateChildEntityGroupOffsets(this, offsetCoordinates);

            // childEntities.ForEach((entity) =>
            // {
            //     entity.relativeOffsetCoordinates = CellCoordinates.Add(entity.relativeOffsetCoordinates, relativeOffsetCoordinatesToAdd);
            // });

            // childEntityGroups.ForEach((entityGroup) =>
            // {
            //     CellCoordinates newEntityGroupOffsetCoordinates = CellCoordinates.Add(entityGroup.offsetCoordinates, relativeOffsetCoordinatesToAdd);
            //     entityGroup.offsetCoordinates = newEntityGroupOffsetCoordinates;
            // });
        }
        /*
            Mutations 
        */
        // Call this after adding this entityGroup as a parent to already-existing entities/entityGroups
        public void UpdateChildrenAfterNewParentAdd()
        {
            // Adjust child offsets to be relative to their new parent
            foreach (Entity entity in childEntities.items)
            {
                entity.relativeOffsetCoordinates = CellCoordinates.Subtract(
                    entity.relativeOffsetCoordinates,
                    this.relativeOffsetCoordinates
                );
            }

            foreach (EntityGroup childEntityGroup in childEntityGroups.items)
            {
                childEntityGroup.relativeOffsetCoordinates = CellCoordinates.Subtract(
                    childEntityGroup.relativeOffsetCoordinates,
                    this.relativeOffsetCoordinates
                );
            }
        }

        // Call this before removing this entityGroup as a parent but preserving children 
        public void UpdateChildrenBeforeParentRemove()
        {
            // Adjust child offsets to be relative to their new parent
            foreach (Entity entity in childEntities.items)
            {
                entity.relativeOffsetCoordinates = CellCoordinates.Add(
                    entity.relativeOffsetCoordinates,
                    this.relativeOffsetCoordinates
                );
            }

            foreach (EntityGroup childEntityGroup in childEntityGroups.items)
            {
                childEntityGroup.relativeOffsetCoordinates = CellCoordinates.Add(
                    childEntityGroup.relativeOffsetCoordinates,
                    this.relativeOffsetCoordinates
                );
            }
        }

        public ListWrapper<Entity> GetDescendantEntities()
        {
            ListWrapper<Entity> result = new ListWrapper<Entity>();

            AddEntityGroupChildEntities(this);

            return result;

            void AddEntityGroupChildEntities(EntityGroup entityGroup)
            {
                result.Add(entityGroup.childEntities);

                foreach (EntityGroup childEntityGroup in entityGroup.childEntityGroups.items)
                {
                    AddEntityGroupChildEntities(childEntityGroup);
                }
            }
        }

        public ListWrapper<EntityGroup> GetDescendantEntityGroups()
        {
            ListWrapper<EntityGroup> result = new ListWrapper<EntityGroup>();

            AddEntityGroup(this);

            return result;

            void AddEntityGroup(EntityGroup entityGroup)
            {
                result.Add(entityGroup);

                foreach (EntityGroup childEntityGroup in entityGroup.childEntityGroups.items)
                {
                    AddEntityGroup(childEntityGroup);
                }
            }
        }

        public Dictionary<Type, ListWrapper<Entity>> GetGroupedEntities()
        {
            Dictionary<Type, ListWrapper<Entity>> groupedEntities = new Dictionary<Type, ListWrapper<Entity>>();

            foreach (Entity entity in GetDescendantEntities().items)
            {
                if (!groupedEntities.ContainsKey(entity.GetType()))
                {
                    groupedEntities[entity.GetType()] = new ListWrapper<Entity>();
                }

                groupedEntities[entity.GetType()].Add(entity);
            }

            return groupedEntities;
        }

        /*
            Static Interface
        */
        public static EntityGroup CreateFromDefinition(EntityGroupDefinition entityGroupDefinition, AppState appState)
        {
            EntityGroupBuilderBase newEntityGroupBuilder = entityGroupDefinition.builderFactory(entityGroupDefinition);
            EntityGroup newEntityGroup = newEntityGroupBuilder.Build(appState);
            return newEntityGroup;
        }
    }
}