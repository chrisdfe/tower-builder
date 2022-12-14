using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    public class GameWorldResident : MonoBehaviour, IGameWorldEntity
    {
        public Resident resident;
        public ResidentBehavior residentBehavior;

        public CurrentAndNext<(TimeValue, CellCoordinates)> currentAndNextPosition;

        public EntityMeshWrapper entityMeshWrapper { get; private set; }

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
            entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, resident.cellCoordinatesList);
            entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, resident.cellCoordinatesList);
            entityMeshWrapper.Setup();

            UpdatePosition();
        }

        public void Teardown() { }

        /* 
            Public interface
        */
        public void UpdatePosition()
        {
            // TODO - use all all coordinates in cellCoordinatesList
            // transform.position =
            //     GameWorldUtils.CellCoordinatesToPosition(resident.cellCoordinatesList.bottomLeftCoordinates);
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

                transform.position = Vector3.Lerp(
                    GameWorldUtils.CellCoordinatesToPosition(startCoordinates),
                    GameWorldUtils.CellCoordinatesToPosition(endCoordinates),
                    normalizedTickProgress
                );
            }
        }

        /* 
            Static API
         */
        public static GameWorldResident Create(Transform parent)
        {
            GameWorldResidentsManager residentsManager = GameWorldResidentsManager.Find();
            GameObject prefab = residentsManager.assetList.FindByKey(GameWorldResidentsManager.AssetKey.Resident);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;

            GameWorldResident gameWorldResident = gameObject.GetComponent<GameWorldResident>();
            return gameWorldResident;
        }
    }
}
