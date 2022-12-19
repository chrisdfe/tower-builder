using TowerBuilder.DataTypes.Entities.Wheels;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Wheels
{
    public class GameWorldWheelsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Wheel
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<Wheel.SkinKey> meshAssets = new MeshAssetList<Wheel.SkinKey>();

        /* 
            Static API
        */
        public static GameWorldWheelsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWheelsManager>("WheelsManager");
    }
}
