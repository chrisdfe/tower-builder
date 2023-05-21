using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    [RequireComponent(typeof(GameWorldEntityList))]
    public class GameWorldRoomsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Room,
            RoomCell
        };

        public AssetList<AssetKey> prefabAssets = new AssetList<AssetKey>();
        // public MeshAssetList<Room.SkinKey> meshAssets = new MeshAssetList<Room.SkinKey>();

        GameWorldEntityList gameWorldEntityList;

        void Awake()
        {
            gameWorldEntityList = GetComponent<GameWorldEntityList>();
            // meshAssets.ReplaceMaterials();
        }

        void Start()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            // Registry.appState.Entities.Rooms.events.onRoomBlocksAdded += OnRoomBlocksAdded;
            // Registry.appState.Entities.Rooms.events.onRoomBlocksRemoved += OnRoomBlocksRemoved;
        }

        public void Teardown()
        {
            // Registry.appState.Entities.Rooms.events.onRoomBlocksAdded -= OnRoomBlocksAdded;
            // Registry.appState.Entities.Rooms.events.onRoomBlocksRemoved -= OnRoomBlocksRemoved;
        }

        /*
            Internals
        */
        void OnRoomBlocksAdded(Room room, CellCoordinatesBlockList blockList)
        {
            // gameWorldEntityList.ResetEntity(room);
        }

        void OnRoomBlocksRemoved(Room room, CellCoordinatesBlockList blockList)
        {
            // gameWorldEntityList.ResetEntity(room);
        }


        /* 
            Static API
        */
        public static GameWorldRoomsManager Find() =>
            GameWorldFindableCache.Find<GameWorldRoomsManager>("RoomsManager");
    }
}