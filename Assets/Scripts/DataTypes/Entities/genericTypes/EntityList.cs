using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityList : ListWrapper<Entity>
    {
        public EntityList() : base() { }
        public EntityList(Entity entity) : base(entity) { }
        public EntityList(List<Entity> entity) : base(entity) { }
        public EntityList(EntityList entityList) : base(entityList) { }
    }

}