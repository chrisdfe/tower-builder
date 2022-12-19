using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    public class GameWorldRoomsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Room,
            RoomCell
        };

        public AssetList<AssetKey> prefabAssets = new AssetList<AssetKey>();

        public MeshAssetList<Room.SkinKey> meshAssets = new MeshAssetList<Room.SkinKey>();

        void Awake()
        {
            meshAssets.ReplaceMaterials();
        }

        /* 
            Static API
        */
        public static GameWorldRoomsManager Find() =>
            GameWorldFindableCache.Find<GameWorldRoomsManager>("RoomsManager");
    }
}