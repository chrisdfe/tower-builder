using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldFloor : MonoBehaviour
    {
        public Floor floor { get; set; }

        EntityMeshWrapper entityMeshWrapper;
        Transform cube;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList<Floor.Key> assetList = GameWorldFloorsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.FindByKey(floor.key);

            // entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, resident.cellCoordinatesList);
            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = floor.cellCoordinatesList;
            entityMeshWrapper.Setup();
        }

        public void Teardown() { }

        /* 
            Static API
         */
        public static GameWorldFloor Create(Transform parent)
        {
            GameWorldFloorsManager floorsManager = GameWorldFloorsManager.Find();
            GameObject prefab = floorsManager.assetList.FindByKey(GameWorldFloorsManager.AssetKey.Floor);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldFloor gameWorldFloor = gameObject.GetComponent<GameWorldFloor>();
            return gameWorldFloor;
        }
    }
}
