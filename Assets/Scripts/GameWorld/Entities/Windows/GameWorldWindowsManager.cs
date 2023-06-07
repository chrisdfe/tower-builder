using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Windows
{
    public class GameWorldWindowsManager : MonoBehaviour, IFindable
    {
        public AssetList assetList = new AssetList();

        public MeshAssetList meshAssets = new MeshAssetList();

        /* 
            Static API
        */
        public static GameWorldWindowsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWindowsManager>("WindowsManager");
    }
}
