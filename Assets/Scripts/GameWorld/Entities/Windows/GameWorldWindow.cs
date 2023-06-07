using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Windows;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Windows
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldWindow : MonoBehaviour
    {
        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        Window window;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void Start()
        {
            window = GetComponent<GameWorldEntity>().entity as Window;

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            AssetList assetList = GameWorldWindowsManager.Find().meshAssets;

            GameObject prefabMesh = assetList.ValueFromKey(window.definition.key);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = window.cellCoordinatesList;
            entityMeshWrapper.Setup();

            GameWorldEntity gameWorldEntity = GetComponent<GameWorldEntity>();
            gameWorldEntity.Setup();
        }

        public void Teardown() { }

        /* 
            Static API
         */
        public static GameWorldWindow Create(Transform parent)
        {
            GameWorldWindowsManager windowsManager = GameWorldWindowsManager.Find();
            GameObject prefab = windowsManager.assetList.ValueFromKey("Window");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldWindow gameWorldWindow = gameObject.GetComponent<GameWorldWindow>();
            return gameWorldWindow;
        }
    }
}
