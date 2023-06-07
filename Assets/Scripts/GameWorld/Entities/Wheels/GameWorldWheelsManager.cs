using TowerBuilder.DataTypes.Entities.Wheels;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Wheels
{
    public class GameWorldWheelsManager : MonoBehaviour, IFindable
    {
        public AssetList assetList = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        /* 
            Static API
        */
        public static GameWorldWheelsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWheelsManager>("WheelsManager");
    }
}
