using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Foundations;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Foundations
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldFoundation : MonoBehaviour
    {
        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        Foundation foundation;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void Start()
        {
            foundation = GetComponent<GameWorldEntity>().entity as Foundation;

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList assetList = GameWorldFoundationsManager.Find().meshAssets;

            string assetKey = foundation.definition.key != null ? foundation.definition.key : "Default";
            GameObject prefabMesh = assetList.ValueFromKey(assetKey);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = foundation.absoluteCellCoordinatesList;

            entityMeshWrapper.CreateEntityMeshCellWrapper = (
               Transform parent,
               GameObject prefabMesh,
               CellCoordinates cellCoordinates,
               CellCoordinates relativeCellCoordinates,
               CellNeighbors cellNeighbors,
               Tileable.CellPosition cellPosition
            ) =>
               new FoundationMeshCellWrapper(parent, prefabMesh, cellCoordinates, relativeCellCoordinates, cellNeighbors, cellPosition);

            entityMeshWrapper.Setup();

            GetComponent<GameWorldEntity>().Setup();
        }

        public void Teardown() { }

        /* 
            Static API
         */
        public static GameWorldFoundation Create(Transform parent)
        {
            GameWorldFoundationsManager foundationsManager = GameWorldFoundationsManager.Find();
            GameObject prefab = foundationsManager.assetList.ValueFromKey("Foundation");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldFoundation gameWorldFoundation = gameObject.GetComponent<GameWorldFoundation>();
            return gameWorldFoundation;
        }
    }
}
