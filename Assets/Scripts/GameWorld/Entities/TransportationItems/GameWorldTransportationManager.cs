using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.TransportationItems
{
    public class GameWorldTransportationManager : MonoBehaviour, IFindable
    {
        public AssetList prefabAssets = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        void Awake()
        {
            meshAssets.ReplaceMaterials();
        }

        public static GameWorldTransportationManager Find() =>
            GameWorldFindableCache.Find<GameWorldTransportationManager>("TransportationManager");
    }
}