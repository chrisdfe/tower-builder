using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable, IValidatable
    {
        public ListWrapper<Entity> entities { get; } = new ListWrapper<Entity>();
        public ListWrapper<EntityGroup> entityGroups { get; } = new ListWrapper<EntityGroup>();

        public bool isInBlueprintMode { get; private set; } = false;

        public bool isValid => validationErrors.Count == 0;
        public ListWrapper<ValidationError> validationErrors { get; private set; } = new ListWrapper<ValidationError>();

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

        public EntityGroup() { }

        public EntityGroup(EntityGroupDefinition definition) { }


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

            // TODO - same for entitygroups

            this.validationErrors = errors;
        }

        /*
            Public API
        */
        public void Add(Entity entity)
        {
            entities.Add(entity);
            entity.isInBlueprintMode = isInBlueprintMode;
        }

        public void Add(ListWrapper<Entity> entitiesList)
        {
            entities.Add(entitiesList);
            entitiesList.ForEach(entity =>
            {
                entity.isInBlueprintMode = isInBlueprintMode;
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

        public void SetBlueprintMode(bool isInBlueprintMode)
        {
            this.isInBlueprintMode = isInBlueprintMode;
            entities.ForEach(entity =>
            {
                entity.isInBlueprintMode = isInBlueprintMode;
            });
        }

        /*
            Queries
        */
        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            new ListWrapper<Entity>();

        /*
            Static API
        */
        public static EntityGroup CreateFromDefinition(EntityGroupDefinition entityGroupDefinition)
        {
            return new EntityGroup(entityGroupDefinition);
        }
    }
}