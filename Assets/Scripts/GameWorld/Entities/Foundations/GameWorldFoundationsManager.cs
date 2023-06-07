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

        public AssetList assetList = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        /* 
            Static API
        */
        public static GameWorldFoundationsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFoundationsManager>("FoundationsManager");
    }
}
