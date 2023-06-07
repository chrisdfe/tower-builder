using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    public class GameWorldFurnitureManager : MonoBehaviour, IFindable
    {
        public AssetList prefabAssets = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        void Awake()
        {
            ReplaceMaterials();
        }

        void ReplaceMaterials()
        {
            foreach (MeshAssetList.ValueTypeWrapper valueTypeWrapper in meshAssets.list)
            {
                GameObject gameObject = valueTypeWrapper.value;
                MaterialsReplacer.ReplaceMaterials(gameObject.transform);
            }
        }

        public static GameWorldFurnitureManager Find() =>
            GameWorldFindableCache.Find<GameWorldFurnitureManager>("FurnitureManager");
    }
}