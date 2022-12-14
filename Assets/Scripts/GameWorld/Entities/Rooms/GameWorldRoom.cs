using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    [RequireComponent(typeof(RoomEntityMeshWrapper))]
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        RoomEntityMeshWrapper roomEntityMeshWrapper;

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
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
            AssetList<GameWorldRoomsManager.MeshAssetKey> assetList = GameWorldRoomsManager.Find().meshAssets;

            // TODO - I should probably just use Skin.Key instead of GameWorldRoomsManager.MeshAssetKey
            GameWorldRoomsManager.MeshAssetKey key = room.skin.key switch
            {
                Room.Skin.Key.Default => GameWorldRoomsManager.MeshAssetKey.Default,
                Room.Skin.Key.Wheels => GameWorldRoomsManager.MeshAssetKey.Wheels,
                _ => GameWorldRoomsManager.MeshAssetKey.Default
            };

            GameObject prefabMesh = assetList.FindByKey(key);

            roomEntityMeshWrapper = GetComponent<RoomEntityMeshWrapper>();
            roomEntityMeshWrapper.prefabMesh = prefabMesh;
            roomEntityMeshWrapper.cellCoordinatesList = room.cellCoordinatesList;
            roomEntityMeshWrapper.skinKey = room.skin.key;
            roomEntityMeshWrapper.Setup();

            UpdatePosition();
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