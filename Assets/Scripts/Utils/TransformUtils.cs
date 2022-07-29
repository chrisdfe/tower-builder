using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Utils
{
    public static class TransformUtils
    {
        // Take from here: https://answers.unity.com/questions/799429/transformfindstring-no-longer-finds-grandchild.html#answer-799493
        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name == aName)
                {
                    return c;
                }

                foreach (Transform t in c)
                {
                    queue.Enqueue(t);
                }
            }

            return null;
        }

        public static void DestroyChildren(Transform wrapper)
        {
            foreach (Transform child in wrapper)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}