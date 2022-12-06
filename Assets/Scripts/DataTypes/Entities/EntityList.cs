using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Residents;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityList
    {
        public List<Entity> entities { get; private set; } = new List<Entity>();

        public int Count { get { return entities.Count; } }

        public EntityList() { }

        public void Add(Entity entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }

        public void Remove(Entity entity)
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

        public void Remove(Resident resident)
        {
            entities.RemoveAll(entity =>
            {
                return (entity is ResidentEntity) && ((ResidentEntity)entity).resident == resident;
            });
        }

        public bool Contains(Furniture furniture)
        {
            return entities.Find(entity =>
            {
                return (entity is FurnitureEntity) && ((FurnitureEntity)entity).furniture == furniture;
            }) != null;
        }

        public bool Contains(Resident resident)
        {
            return entities.Find(entity =>
            {
                return (entity is ResidentEntity) && ((ResidentEntity)entity).resident == resident;
            }) != null;
        }
    }
}