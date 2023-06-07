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

        public AssetList assetList = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        /* 
            Static API
        */
        public static GameWorldInteriorLightsManager Find() =>
            GameWorldFindableCache.Find<GameWorldInteriorLightsManager>("InteriorLightsManager");
    }
}
