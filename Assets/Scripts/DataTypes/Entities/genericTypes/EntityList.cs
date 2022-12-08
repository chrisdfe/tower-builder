using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityList
    {
        public List<Entity> entities { get; private set; } = new List<Entity>();

        public int Count { get { return entities.Count; } }

        public EntityList() { }

        public void Add<EntityType>(EntityType entity)
            where EntityType : Entity
        {
            if (!Contains<EntityType>(entity))
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
                return (entity is Furniture) && ((Furniture)entity) == furniture;
            });
        }

        public void Remove(Resident resident)
        {
            entities.RemoveAll(entity =>
            {
                return (entity is Resident) && ((Resident)entity) == resident;
            });
        }

        public bool Contains<EntityType>(Entity targetEntity)
            where EntityType : Entity
        =>
            entities.Find(entity =>
                (entity is EntityType) && ((EntityType)entity) == targetEntity
            ) != null;
    }
}