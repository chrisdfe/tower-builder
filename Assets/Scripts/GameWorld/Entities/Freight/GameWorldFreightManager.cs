using TowerBuilder.DataTypes.Entities.Freights;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Freight
{
    public class GameWorldFreightManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Freight
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<FreightItem.Key> meshAssets = new MeshAssetList<FreightItem.Key>();

        /* 
            Static API
        */
        public static GameWorldFreightManager Find() =>
            GameWorldFindableCache.Find<GameWorldFreightManager>("FreightManager");
    }
}
