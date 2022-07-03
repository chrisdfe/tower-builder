using UnityEngine;

namespace TowerBuilder.Utils
{
    public static class MathUtils
    {
        public static int RoundUpToNearest(int number, int nearest)
        {
            return (int)(Mathf.Ceil(((float)number / (float)nearest)) * (float)nearest);
        }
    }
}