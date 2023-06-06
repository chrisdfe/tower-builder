using TowerBuilder.DataTypes.Entities.Foundations;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Foundations
{
    public class GameWorldFoundationsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Foundation
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<Foundation.Key> meshAssets = new MeshAssetList<Foundation.Key>();

        /* 
            Static API
        */
        public static GameWorldFoundationsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFoundationsManager>("FoundationsManager");
    }
}
