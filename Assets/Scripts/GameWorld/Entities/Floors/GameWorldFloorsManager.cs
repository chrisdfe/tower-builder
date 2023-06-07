using TowerBuilder.DataTypes.Entities.Floors;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    public class GameWorldFloorsManager : MonoBehaviour, IFindable
    {
        public AssetList assetList = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        /* 
            Static API
        */
        public static GameWorldFloorsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFloorsManager>("FloorsManager");
    }
}
