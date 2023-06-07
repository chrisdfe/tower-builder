using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class MaterialsManager : MonoBehaviour, IFindable
    {
        public MaterialsList materialList = new MaterialsList();

        public static MaterialsManager Find() =>
            GameWorldFindableCache.Find<MaterialsManager>("MaterialsManager");
    }
}