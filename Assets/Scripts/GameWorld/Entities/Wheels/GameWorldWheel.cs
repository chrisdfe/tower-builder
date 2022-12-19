using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Wheels
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldWheel : MonoBehaviour
    {
        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        Wheel wheel;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void Start()
        {
            wheel = GetComponent<GameWorldEntity>().entity as Wheel;

            Setup();
        }
        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList<Wheel.SkinKey> assetList = GameWorldWheelsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.FindByKey(wheel.skinKey);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = wheel.cellCoordinatesList;
            entityMeshWrapper.Setup();

            GameWorldEntity gameWorldEntity = GetComponent<GameWorldEntity>();
            gameWorldEntity.Setup();
        }

        public void Teardown() { }

        /* 
            Static API
         */
        public static GameWorldWheel Create(Transform parent)
        {
            GameWorldWheelsManager wheelsManager = GameWorldWheelsManager.Find();
            GameObject prefab = wheelsManager.assetList.FindByKey(GameWorldWheelsManager.AssetKey.Wheel);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;

            GameWorldWheel gameWorldWheel = gameObject.GetComponent<GameWorldWheel>();
            return gameWorldWheel;
        }
    }
}
