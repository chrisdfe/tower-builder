using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    [ExecuteInEditMode]
    public class MaterialsReplacer
    {

        public void ReplaceMaterials(Transform parent)
        {
            List<Transform> materialChildren = FindAllChildrenWhereRecursive(parent, (child) =>
                GetChildMaterial(child) != null
            );

            Debug.Log("materialChildren");
            Debug.Log(materialChildren.Count);

            materialChildren.ForEach(child => ReplaceMaterial(child));
        }

        void ReplaceMaterial(Transform child)
        {
            Debug.Log($"replacing material for {child.gameObject.name}");
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            Material material = meshRenderer.sharedMaterial;
            string materialName = material.name;
            string cleanedName = materialName.Replace(" (Instance)", "");
            Debug.Log("cleanedName");
            Debug.Log(cleanedName);
            MaterialsManager materialsManager = MaterialsManager.Find();
            Material replacementMaterial = materialsManager.FindByName(cleanedName);
            Debug.Log("replacementMaterial");
            Debug.Log(replacementMaterial);

            if (replacementMaterial != null)
            {
                meshRenderer.sharedMaterial = replacementMaterial;
            }
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
                }

                if (child.childCount > 0)
                {
                    result = result.Concat(FindAllChildrenWhereRecursive(child, predicate)).ToList();
                }
            }
            return result;
        }

        Material GetChildMaterial(Transform child)
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