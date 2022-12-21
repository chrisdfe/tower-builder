using TowerBuilder.DataTypes.Entities.InteriorLights;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorLights
{
    public class GameWorldInteriorLightsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Light
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<InteriorLight.Key> meshAssets = new MeshAssetList<InteriorLight.Key>();

        /* 
            Static API
        */
        public static GameWorldInteriorLightsManager Find() =>
            GameWorldFindableCache.Find<GameWorldInteriorLightsManager>("InteriorLightsManager");
    }
}
