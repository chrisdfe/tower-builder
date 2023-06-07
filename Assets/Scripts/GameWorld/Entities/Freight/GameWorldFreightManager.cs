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

        public AssetList assetList = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        /* 
            Static API
        */
        public static GameWorldFreightManager Find() =>
            GameWorldFindableCache.Find<GameWorldFreightManager>("FreightManager");
    }
}
