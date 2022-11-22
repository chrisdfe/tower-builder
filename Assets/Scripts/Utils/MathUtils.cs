using UnityEngine;

namespace TowerBuilder.Utils
{
    public static class MathUtils
    {
        public static int RoundUpToNearest(int number, int nearest)
        {
            return (int)(Mathf.Ceil(((float)number / (float)nearest)) * (float)nearest);
        }

        public static float NormalizeFloat(float val, float min, float max)
        {
            return Mathf.Clamp((val - min) / (max - min), 0f, 1f);
        }
    }
}