using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            TransportationItem
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public static GameWorldTransportationManager Find() =>
            GameWorldFindableCache.Find<GameWorldTransportationManager>("TransportationManager");
    }
}