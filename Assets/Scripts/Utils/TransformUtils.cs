using UnityEngine;

namespace TowerBuilder.Utils
{
    public static class TransformUtils
    {
        public static void DestroyChildren(Transform wrapper)
        {
            foreach (Transform child in wrapper)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}