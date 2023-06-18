using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable, IValidatable
    {
        public ListWrapper<Entity> childEntities { get; } = new ListWrapper<Entity>();
        public ListWrapper<EntityGroup> childEntityGroups { get; } = new ListWrapper<EntityGroup>();

        public EntityGroup parent { get; set; } = null;

        public int id { get; }

        public bool isInBlueprintMode { get; private set; } = false;

        public bool isValid => validationErrors.Count == 0;
        public ListWrapper<ValidationError> validationErrors { get; private set; } = new ListWrapper<ValidationError>();

        public virtual string typeLabel => "EntityGroup";

        public int price =>
            descendantEntities.items.Aggregate(0, ((acc, entity) => acc + entity.price));

        public ListWrapper<Entity> descendantEntities
        {
            get
            {
                ListWrapper<Entity> result = new ListWrapper<Entity>();

                AddEntityGroup(this);

                return result;

                void AddEntityGroup(EntityGroup entityGroup)
                {
                    result.Add(entityGroup.childEntities);

                    foreach (EntityGroup childEntityGroup in entityGroup.childEntityGroups.items)
                    {
                        AddEntityGroup(childEntityGroup);
                    }
                }
            }
        }

        public CellCoordinates offsetCoordinates { get; set; } = CellCoordinates.zero;

        public CellCoordinates absoluteOffsetCellCoordinates
        {
            get
            {
                CellCoordinates result = offsetCoordinates;
                EntityGroup currentParent = parent;

                while (currentParent != null)
                {
                    result = CellCoordinates.Add(result, currentParent.offsetCoordinates);
                    currentParent = currentParent.parent;
                }

                return result;
            }
        }

        public Dictionary<Type, ListWrapper<Entity>> groupedEntities
        {
            get
            {
                Dictionary<Type, ListWrapper<Entity>> groupedEntities = new Dictionary<Type, ListWrapper<Entity>>();

                foreach (Entity entity in descendantEntities.items)
                {
                    if (!groupedEntities.ContainsKey(entity.GetType()))
                    {
                        groupedEntities[entity.GetType()] = new ListWrapper<Entity>();
                    }

                    groupedEntities[entity.GetType()].Add(entity);
                }

                return groupedEntities;
            }
        }

        public CellCoordinatesList absoluteCellCoordinatesList
        {
            get
            {
                CellCoordinatesList result = new CellCoordinatesList();

                foreach (Entity entity in descendantEntities.items)
                {
                    result.Add(entity.absoluteCellCoordinatesList.items);
                }

                return result;
            }
        }

        public override string ToString() => $"{typeLabel} #{id}";

        public EntityGroup()
        {
            id = UIDGenerator.Generate(typeLabel);
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

        public void Validate(AppState appState)
        {
            ListWrapper<ValidationError> errors = new ListWrapper<ValidationError>();

            foreach (Entity entity in childEntities.items)
            {
                entity.Validate(appState);
                errors.Add(entity.validationErrors);
            }

            foreach (EntityGroup entityGroup in childEntityGroups.items)
            {
                entityGroup.Validate(appState);
                errors.Add(entityGroup.validationErrors);
            }

            this.validationErrors = errors;
        }

        /*
            Public Interface
        */
        public void Add(Entity entity)
        {
            childEntities.Add(entity);
            entity.parent = this;
            entity.isInBlueprintMode = isInBlueprintMode;
        }

        public void Add(ListWrapper<Entity> entitiesList)
        {
            childEntities.Add(entitiesList);
            entitiesList.ForEach(entity =>
            {
                entity.parent = this;
                entity.isInBlueprintMode = isInBlueprintMode;
            });
        }

        public void Add(EntityGroup entityGroup)
        {
            childEntityGroups.Add(entityGroup);
            entityGroup.parent = this;
            entityGroup.SetBlueprintMode(isInBlueprintMode);
        }

        public void Add(ListWrapper<EntityGroup> entityGroupList)
        {
            childEntityGroups.Add(entityGroupList);
            entityGroupList.ForEach(entityGroup =>
            {
                entityGroup.parent = this;
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

            childEntities.ForEach(entity =>
            {
                entity.isInBlueprintMode = isInBlueprintMode;
            });

            childEntityGroups.ForEach(entityGroup =>
            {
                entityGroup.SetBlueprintMode(isInBlueprintMode);
            });
        }

        /*
            Queries
        */
        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            childEntities.FindAll(entity => entity.absoluteCellCoordinatesList.Contains(cellCoordinates));

        /*
            Static Interface
        */
        public static EntityGroup CreateFromDefinition(EntityGroupDefinition entityGroupDefinition)
        {
            return new EntityGroup(entityGroupDefinition);
        }
    }
}