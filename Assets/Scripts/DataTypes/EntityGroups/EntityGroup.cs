using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroup : ISetupable
    {
        public ListWrapper<Entity> entitiesList { get; }
        public EntityListMap entityListMap { get; }

        public EntityGroup()
        {
        }

        public void Setup() { }

        public void Teardown() { }

        public void Add(Entity entity)
        {
            entitiesList.Add(entity);
            entityListMap.Add(entity);
        }

        public void Remove(Entity entity)
        {
            entitiesList.Remove(entity);
            entityListMap.Remove(entity);
        }

        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            new ListWrapper<Entity>();
    }
}