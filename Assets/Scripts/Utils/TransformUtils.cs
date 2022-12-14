using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Utils
{
    public static class TransformUtils
    {
        // Take from here: https://answers.unity.com/questions/799429/transformfindstring-no-longer-finds-grandchild.html#answer-799493
        //Breadth-first search
        public static Transform FindDeepChild(Transform parent, string targetName) =>
            FindDeepChildWhere(parent, (Transform child) => child.name == targetName);

        public delegate bool FindChildWherePredicate(Transform child);
        public static Transform FindDeepChildWhere(Transform parent, FindChildWherePredicate predicate)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(parent);

            while (queue.Count > 0)
            {
                var child = queue.Dequeue();
                if (predicate(child))
                {
                    return child;
                }

                foreach (Transform t in child)
                {
                    queue.Enqueue(t);
                }
            }

            return null;
        }

        public static List<Transform> FindDeepChildrenWhere(Transform parent, FindChildWherePredicate predicate)
        {
            List<Transform> result = new List<Transform>();
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(parent);

            while (queue.Count > 0)
            {
                var child = queue.Dequeue();
                if (predicate(child))
                {
                    result.Add(child);
                }

                foreach (Transform t in child)
                {
                    queue.Enqueue(t);
                }
            }

            return result;
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