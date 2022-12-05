using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class MaterialsReplacer : MonoBehaviour
    {
        MaterialsManager materialsManager;

        void Awake()
        {
            materialsManager = MaterialsManager.Find();
        }

        public void ReplaceMaterials()
        {
            List<Transform> allChildren = FindAllChildrenWhereRecursive(transform, (transform) => true);
            Debug.Log("allChildren");
            Debug.Log(allChildren);
            Debug.Log(allChildren.Count);
        }

        // TODO - put in TransformUtils
        public delegate bool Predicate(Transform transform);
        List<Transform> FindAllChildrenWhereRecursive(Transform parent, Predicate predicate)
        {
            List<Transform> result = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (predicate(child))
                {
                    result.Add(child);

                    if (child.childCount > 0)
                    {
                        result = result.Concat(FindAllChildrenWhereRecursive(child, predicate)).ToList();
                    }
                }
            }
            return result;
        }
    }
}