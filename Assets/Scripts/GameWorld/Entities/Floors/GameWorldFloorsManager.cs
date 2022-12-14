using TowerBuilder.DataTypes.Entities.Floors;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    public class GameWorldFloorsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Floor
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<Floor.Key> meshAssets = new MeshAssetList<Floor.Key>();

        /* 
            Static API
        */
        public static GameWorldFloorsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFloorsManager>("FloorsManager");
    }
}
