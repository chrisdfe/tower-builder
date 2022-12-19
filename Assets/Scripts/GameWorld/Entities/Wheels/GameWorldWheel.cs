using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Wheels
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldWheel : MonoBehaviour
    {
        public Wheel wheel;

        public CurrentAndNext<(TimeValue, CellCoordinates)> currentAndNextPosition;

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

        void Update()
        {
            UpdateMovement();
        }

        public void Setup()
        {
            AssetList<Wheel.SkinKey> assetList = GameWorldWheelsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.FindByKey(wheel.skinKey);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = wheel.cellCoordinatesList;
            entityMeshWrapper.Setup();
        }

        public void Teardown() { }

        public void UpdatePosition()
        {
            entityMeshWrapper.cellCoordinatesList = wheel.cellCoordinatesList;
            entityMeshWrapper.UpdatePosition();
        }

        /* 
            Internals
        */
        void UpdateMovement()
        {
            if (currentAndNextPosition != null)
            {
                var ((startTick, startCoordinates), (endTick, endCoordinates)) = currentAndNextPosition;
                float normalizedTickProgress = GameWorldTimeSystemManager.Find().normalizedTickProgress;

                entityMeshWrapper.SetPosition(Vector3.Lerp(
                    GameWorldUtils.CellCoordinatesToPosition(startCoordinates),
                    GameWorldUtils.CellCoordinatesToPosition(endCoordinates),
                    normalizedTickProgress
                ));
            }
        }

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
