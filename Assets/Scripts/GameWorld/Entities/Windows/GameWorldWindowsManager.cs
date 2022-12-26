using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Windows
{
    public class GameWorldWindowsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Window
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<Window.Key> meshAssets = new MeshAssetList<Window.Key>();

        /* 
            Static API
        */
        public static GameWorldWindowsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWindowsManager>("WindowsManager");
    }
}
