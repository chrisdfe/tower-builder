using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures;

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

        public void Remove(EntityBase entity)
        {
            entities.Remove(entity);
        }

        public void Remove(Furniture furniture)
        {
            entities.RemoveAll(entity =>
            {
                return (entity is FurnitureEntity) && ((FurnitureEntity)entity).furniture == furniture;
            });
        }

        public bool Contains(Furniture furniture)
        {
            return entities.Find(entity =>
            {
                return (entity is FurnitureEntity) && ((FurnitureEntity)entity).furniture == furniture;
            }) != null;
        }
    }
}