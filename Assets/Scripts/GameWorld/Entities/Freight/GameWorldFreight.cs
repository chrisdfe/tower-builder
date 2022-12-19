using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Freight
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldFreight : MonoBehaviour
    {
        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        FreightItem freightItem;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void Start()
        {
            Debug.Log("Start");
            freightItem = GetComponent<GameWorldEntity>().entity as FreightItem;
            Debug.Log("freightItem");
            Debug.Log(freightItem);
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList<FreightItem.Key> assetList = GameWorldFreightManager.Find().meshAssets;

            Debug.Log("freightItem");
            Debug.Log(freightItem);
            Debug.Log(freightItem.key);

            GameObject prefabMesh = assetList.FindByKey(freightItem.key);
            Debug.Log("prefabMesh");
            Debug.Log(prefabMesh);

            // entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, resident.cellCoordinatesList);
            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = freightItem.cellCoordinatesList;
            entityMeshWrapper.Setup();

            GetComponent<GameWorldEntity>().Setup();
        }

        public void Teardown() { }

        /* 
            Static API
         */
        public static GameWorldFreight Create(Transform parent)
        {
            GameWorldFreightManager freightsManager = GameWorldFreightManager.Find();
            GameObject prefab = freightsManager.assetList.FindByKey(GameWorldFreightManager.AssetKey.Freight);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldFreight gameWorldFreight = gameObject.GetComponent<GameWorldFreight>();
            return gameWorldFreight;
        }
    }
}
