using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityListMap
    {
        public Dictionary<Type, ListWrapper<Entity>> map { get; } = new Dictionary<Type, ListWrapper<Entity>>();

        public ListWrapper<Entity> Get(Type type) => map[type];

        public void Add(Entity entity)
        {
            Type entityType = entity.GetType();
            if (!map.ContainsKey(entityType))
            {
                map[entityType] = new ListWrapper<Entity>();
            }

            map[entityType].Add(entity);
        }

        public void Add(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                Add(entity);
            }
        }

        public void Remove(Entity entity)
        {
            Type entityType = entity.GetType();

            if (map.ContainsKey(entityType))
            {
                map[entityType].Remove(entity);
            }
        }

        public void Remove(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                Remove(entity);
            }
        }
    }
}