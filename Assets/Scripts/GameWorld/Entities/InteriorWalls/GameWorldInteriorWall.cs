using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.InteriorWalls;
using TowerBuilder.DataTypes.Entities.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorWalls
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldInteriorWall : MonoBehaviour
    {
        public InteriorWall interiorWall { get; set; }

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
            AssetList<InteriorWall.Key> assetList = GameWorldInteriorWallsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.FindByKey(interiorWall.key);

            // entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, resident.cellCoordinatesList);
            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = interiorWall.cellCoordinatesList;
            entityMeshWrapper.Setup();
        }

        public void Teardown() { }

        /* 
            Static API
         */
        public static GameWorldInteriorWall Create(Transform parent)
        {
            GameWorldInteriorWallsManager interiorWallsManager = GameWorldInteriorWallsManager.Find();
            GameObject prefab = interiorWallsManager.assetList.FindByKey(GameWorldInteriorWallsManager.AssetKey.InteriorWall);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldInteriorWall gameWorldInteriorWall = gameObject.GetComponent<GameWorldInteriorWall>();
            return gameWorldInteriorWall;
        }
    }
}
