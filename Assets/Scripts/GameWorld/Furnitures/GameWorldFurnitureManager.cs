using UnityEngine;

namespace TowerBuilder.GameWorld.Furnitures
{
    public class GameWorldFurnitureManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Furniture,
            Bed,
            Engine
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public static GameWorldFurnitureManager Find() =>
            GameWorldFindableCache.Find<GameWorldFurnitureManager>("FurnitureManager");
    }
}