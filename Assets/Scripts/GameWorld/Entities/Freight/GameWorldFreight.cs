using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Freights;
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
            freightItem = GetComponent<GameWorldEntity>().entity as FreightItem;

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList assetList = GameWorldFreightManager.Find().meshAssets;

            GameObject prefabMesh = assetList.ValueFromKey(freightItem.definition.key);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = freightItem.absoluteCellCoordinatesList;
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
            GameObject prefab = freightsManager.assetList.ValueFromKey("Freight");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldFreight gameWorldFreight = gameObject.GetComponent<GameWorldFreight>();
            return gameWorldFreight;
        }
    }
}
