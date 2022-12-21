using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldFloor : MonoBehaviour
    {
        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        Floor floor;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void Start()
        {
            floor = GetComponent<GameWorldEntity>().entity as Floor;

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList<Floor.Key> assetList = GameWorldFloorsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.FindByKey(floor.key);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = floor.cellCoordinatesList;
            entityMeshWrapper.Setup();

            GetComponent<GameWorldEntity>().Setup();
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
