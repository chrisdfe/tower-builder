using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable, IValidatable
    {
        public ListWrapper<Entity> entities { get; } = new ListWrapper<Entity>();
        public ListWrapper<EntityGroup> entityGroups { get; } = new ListWrapper<EntityGroup>();

        public EntityGroup parent { get; set; } = null;

        public int id { get; }

        public bool isInBlueprintMode { get; private set; } = false;

        public bool isValid => validationErrors.Count == 0;
        public ListWrapper<ValidationError> validationErrors { get; private set; } = new ListWrapper<ValidationError>();

        public virtual string typeLabel => "EntityGroup";

        public int price =>
            entities.items.Aggregate(0, ((acc, entity) => acc + entity.price));

        public CellCoordinates offsetCoordinates { get; set; } = CellCoordinates.zero;

        public CellCoordinates absoluteCellCoordinates
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

                foreach (Entity entity in entities.items)
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

            foreach (Entity entity in entities.items)
            {
                entity.Validate(appState);
                errors.Add(entity.validationErrors);
            }

            foreach (EntityGroup entityGroup in entityGroups.items)
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
            entities.Add(entity);
            entity.parent = this;
            entity.isInBlueprintMode = isInBlueprintMode;
        }

        public void Add(ListWrapper<Entity> entitiesList)
        {
            entities.Add(entitiesList);
            entitiesList.ForEach(entity =>
            {
                entity.parent = this;
                entity.isInBlueprintMode = isInBlueprintMode;
            });
        }

        public void Add(EntityGroup entityGroup)
        {
            entityGroups.Add(entityGroup);
            entityGroup.parent = this;
            entityGroup.SetBlueprintMode(isInBlueprintMode);
        }

        public void Add(ListWrapper<EntityGroup> entityGroupList)
        {
            entityGroups.Add(entityGroupList);
            entityGroupList.ForEach(entityGroup =>
            {
                entityGroup.parent = this;
                entityGroup.SetBlueprintMode(isInBlueprintMode);
            });
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
        }

        public void Remove(ListWrapper<Entity> entitiesList)
        {
            entities.Remove(entitiesList);
        }

        public void Remove(EntityGroup entityGroup)
        {
            entityGroups.Remove(entityGroup);
        }

        public void Remove(ListWrapper<EntityGroup> entityGroups)
        {
            entityGroups.Remove(entityGroups);
        }

        public void SetBlueprintMode(bool isInBlueprintMode)
        {
            this.isInBlueprintMode = isInBlueprintMode;

            entities.ForEach(entity =>
            {
                entity.isInBlueprintMode = isInBlueprintMode;
            });

            entityGroups.ForEach(entityGroup =>
            {
                entityGroup.SetBlueprintMode(isInBlueprintMode);
            });
        }

        /*
            Queries
        */
        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            entities.FindAll(entity => entity.absoluteCellCoordinatesList.Contains(cellCoordinates));

        /*
            Static Interface
        */
        public static EntityGroup CreateFromDefinition(EntityGroupDefinition entityGroupDefinition)
        {
            return new EntityGroup(entityGroupDefinition);
        }
    }
}