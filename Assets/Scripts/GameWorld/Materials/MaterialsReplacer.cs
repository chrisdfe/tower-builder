using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    [ExecuteInEditMode]
    public class MaterialsReplacer
    {
        public static void ReplaceMaterials(Transform parent)
        {
            MaterialsManager materialsManager = MaterialsManager.Find();

            List<Transform> materialChildren = TransformUtils.FindDeepChildrenWhere(parent, (child) =>
                GetChildMaterial(child) != null
            );

            materialChildren.ForEach(child => ReplaceMaterial(child, materialsManager));
        }

        static void ReplaceMaterial(Transform child, MaterialsManager materialsManager)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            Material material = meshRenderer.sharedMaterial;
            string materialName = material.name;
            string cleanedName = materialName.Replace(" (Instance)", "").ToLower();

            Debug.Log("cleanedName");
            Debug.Log(cleanedName);

            Material replacementMaterial = materialsManager.materialList.ValueFromKey(cleanedName);

            Debug.Log("replacementMaterial");
            Debug.Log(replacementMaterial);

            if (replacementMaterial != null)
            {
                meshRenderer.sharedMaterial = replacementMaterial;
            }
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