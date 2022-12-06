using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    [ExecuteInEditMode]
    public class MaterialsReplacer
    {
        public static void ReplaceMaterials(Transform parent)
        {
            List<Transform> materialChildren = FindAllChildrenWhereRecursive(parent, (child) =>
                GetChildMaterial(child) != null
            );

            materialChildren.ForEach(child => ReplaceMaterial(child));
        }

        static void ReplaceMaterial(Transform child)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            Material material = meshRenderer.sharedMaterial;
            string materialName = material.name;
            string cleanedName = materialName.Replace(" (Instance)", "");
            MaterialsManager materialsManager = MaterialsManager.Find();
            Material replacementMaterial = materialsManager.FindByName(cleanedName);
            if (replacementMaterial != null)
            {
                meshRenderer.sharedMaterial = replacementMaterial;
            }
        }

        // TODO - put in TransformUtils
        public delegate bool Predicate(Transform transform);
        static List<Transform> FindAllChildrenWhereRecursive(Transform parent, Predicate predicate)
        {
            List<Transform> result = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (predicate(child))
                {
                    result.Add(child);
                }

                if (child.childCount > 0)
                {
                    result = result.Concat(FindAllChildrenWhereRecursive(child, predicate)).ToList();
                }
            }
            return result;
        }

        static Material GetChildMaterial(Transform child)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                Material material = meshRenderer.sharedMaterial;

                if (material != null)
                {
                    return material;
                }
            }

            return null;
        }
    }
}