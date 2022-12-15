using TowerBuilder.DataTypes.Entities.InteriorWalls;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorWalls
{
    public class GameWorldInteriorWallsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            InteriorWall
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<InteriorWall.Key> meshAssets = new MeshAssetList<InteriorWall.Key>();

        /* 
            Static API
        */
        public static GameWorldInteriorWallsManager Find() =>
            GameWorldFindableCache.Find<GameWorldInteriorWallsManager>("InteriorWallsManager");
    }
}
