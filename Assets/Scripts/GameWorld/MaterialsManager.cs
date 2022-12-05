using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class MaterialsManager : MonoBehaviour, IFindable
    {
        public enum MaterialKey
        {
            Primary,
            Secondary,
            Glass,
        }

        public MaterialsList<MaterialKey> materialList = new MaterialsList<MaterialKey>();

        public Material FindByName(string name)
        {
            MaterialsList<MaterialKey>.ValueTypeWrapper wrapper =
                materialList.assetList.Find(wrapper =>
                    wrapper.key.ToString().ToLower() == name.ToLower()
                );

            if (wrapper != null)
            {
                return wrapper.value;
            }

            return null;
        }

        public static MaterialsManager Find() =>
            GameWorldFindableCache.Find<MaterialsManager>("MaterialsManager");
    }
}