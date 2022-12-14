using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    public class GameWorldFurnitureManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Furniture,
        }

        public AssetList<AssetKey> prefabAssets = new AssetList<AssetKey>();

        public enum MeshAssetKey
        {
            Bed,
            Engine,
            PilotSeat,
            MoneyMachine
        };

        public MeshAssetList<MeshAssetKey> meshAssets = new MeshAssetList<MeshAssetKey>();

        void Awake()
        {
            ReplaceMaterials();
        }

        void ReplaceMaterials()
        {
            foreach (AssetList<MeshAssetKey>.ValueTypeWrapper wrapper in meshAssets.list)
            {
                GameObject gameObject = wrapper.value;
                MaterialsReplacer.ReplaceMaterials(gameObject.transform);
            }
        }

        public static GameWorldFurnitureManager Find() =>
            GameWorldFindableCache.Find<GameWorldFurnitureManager>("FurnitureManager");
    }
}