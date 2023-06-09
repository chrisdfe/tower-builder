using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable
    {
        public ListWrapper<Entity> entities { get; }

        public ListWrapper<EntityGroup> entitiyGroups { get; }

        public EntityGroup() { }

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

        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            new ListWrapper<Entity>();
    }
}