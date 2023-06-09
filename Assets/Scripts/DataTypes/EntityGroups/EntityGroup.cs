using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable
    {
        public ListWrapper<Entity> entities { get; } = new ListWrapper<Entity>();

        // public ListWrapper<EntityGroup> entityGroups { get; } new ListWrapper<EntityGroup>();

        public EntityGroup() { }
        public EntityGroup(EntityGroupDefinition definition) { }

        public bool isInBlueprintMode = false;

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

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public void Add(Entity entity)
        {
            entities.Add(entity);
        }

        public void Add(ListWrapper<Entity> entitiesList)
        {
            entities.Add(entitiesList);
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
        }

        public void Remove(ListWrapper<Entity> entitiesList)
        {
            entities.Remove(entitiesList);
        }

        public virtual void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public virtual void OnDestroy() { }

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