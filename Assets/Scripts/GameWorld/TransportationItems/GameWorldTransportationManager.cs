using TowerBuilder.DataTypes.TransportationItems;
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

        public AssetList<TransportationItem.Key> meshAssetList = new AssetList<TransportationItem.Key>();

        public static GameWorldTransportationManager Find() =>
            GameWorldFindableCache.Find<GameWorldTransportationManager>("TransportationManager");
    }
}