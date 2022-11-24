using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes
{
    public class CurrentAndNext<T>
    {
        public T current;
        public T next;

        public void Deconstruct(out T current, out T next)
        {
            current = this.current;
            next = this.next;
        }
    }
}