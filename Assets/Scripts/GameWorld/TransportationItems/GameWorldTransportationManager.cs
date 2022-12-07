using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            TransportationItem
        }

        public AssetList<AssetKey> prefabAssets = new AssetList<AssetKey>();

        public MeshAssetList<TransportationItem.Key> meshAssets = new MeshAssetList<TransportationItem.Key>();

        void Awake()
        {
            meshAssets.ReplaceMaterials();
        }

        public static GameWorldTransportationManager Find() =>
            GameWorldFindableCache.Find<GameWorldTransportationManager>("TransportationManager");
    }
}