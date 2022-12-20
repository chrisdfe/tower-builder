using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Vehicles
{
    [RequireComponent(typeof(GameWorldEntityList))]
    public class GameWorldVehiclesManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Vehicle
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public MeshAssetList<Vehicle.Key> meshAssets = new MeshAssetList<Vehicle.Key>();

        /* 
            Static API
        */
        public static GameWorldVehiclesManager Find() =>
            GameWorldFindableCache.Find<GameWorldVehiclesManager>("VehiclesManager");
    }
}
