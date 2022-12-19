using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    [RequireComponent(typeof(GameWorldEntity))]
    [RequireComponent(typeof(RoomEntityMeshWrapper))]
    public class GameWorldRoom : MonoBehaviour
    {
        RoomEntityMeshWrapper roomEntityMeshWrapper;
        Room room;

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void Start()
        {
            room = GetComponent<GameWorldEntity>().entity as Room;

            Setup();
        }

        // When this has been converted from a blueprint room to a actual room
        public void OnBuild()
        {
        }

        void OnDestroy()
        {
        }

        public void Setup()
        {
            AssetList<Room.SkinKey> meshAssets = GameWorldRoomsManager.Find().meshAssets;

            GameObject prefabMesh = meshAssets.FindByKey(room.skinKey);

            roomEntityMeshWrapper = GetComponent<RoomEntityMeshWrapper>();
            roomEntityMeshWrapper.prefabMesh = prefabMesh;
            roomEntityMeshWrapper.cellCoordinatesList = room.cellCoordinatesList;
            roomEntityMeshWrapper.Setup();

            UpdatePosition();

            GameWorldEntity gameWorldEntity = GetComponent<GameWorldEntity>();
            gameWorldEntity.customMeshWrapper = roomEntityMeshWrapper;
            gameWorldEntity.Setup();
        }

        public void Teardown()
        {
        }

        public void Reset()
        {
            roomEntityMeshWrapper.Reset();
        }

        /* 
            Public Interface
        */
        public void UpdatePosition()
        {
            // TODO - use all all coordinates in cellCoordinatesList
            // transform.position = GameWorldUtils.CellCoordinatesToPosition(room.cellCoordinatesList.bottomLeftCoordinates);
        }

        public void SetRoom(Room room)
        {
            this.room = room;
            gameObject.name = $"Room {room.id}";
        }

        /* 
            Static API
         */
        public static GameWorldRoom Create(Transform parent)
        {
            GameWorldRoomsManager roomsManager = GameWorldRoomsManager.Find();
            GameObject prefab = roomsManager.prefabAssets.FindByKey(GameWorldRoomsManager.AssetKey.Room);
            GameObject roomGameObject = Instantiate<GameObject>(prefab);

            roomGameObject.transform.parent = parent;

            GameWorldRoom gameWorldRoom = roomGameObject.GetComponent<GameWorldRoom>();
            return gameWorldRoom;
        }
    }
}