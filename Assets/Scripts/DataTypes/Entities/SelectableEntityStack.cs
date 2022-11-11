using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities
{
    public class SelectableEntityStack
    {
        public Stack<EntityBase> entities { get; private set; } = new Stack<EntityBase>();

        public int Count { get { return entities.Count; } }

        public SelectableEntityStack() { }

        public void Push(EntityBase entity)
        {
            entities.Push(entity);
        }

        public EntityBase Pop()
        {
            return entities.Pop();
        }
    }
}