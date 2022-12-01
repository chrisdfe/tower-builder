using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes
{
    public class MinMax<T>
    {
        public T min;
        public T max;

        public MinMax() { }

        public MinMax(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        public void Deconstruct(out T min, out T max)
        {
            min = this.min;
            max = this.max;
        }
    }
}