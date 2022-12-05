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

        public static MaterialsManager Find() =>
            GameWorldFindableCache.Find<MaterialsManager>("MaterialsManager");

    }
}