using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityList
    {
        public List<EntityBase> entities { get; private set; } = new List<EntityBase>();

        public int Count { get { return entities.Count; } }

        public EntityList() { }

        public void Add(EntityBase entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }
    }
}