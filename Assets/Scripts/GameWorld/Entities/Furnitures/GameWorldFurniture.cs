using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    [RequireComponent(typeof(GameWorldEntity))]
    public class GameWorldFurniture : MonoBehaviour
    {

        EntityMeshWrapper entityMeshWrapper;
        Transform cube;
        Furniture furniture;

        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        void Start()
        {
            furniture = GetComponent<GameWorldEntity>().entity as Furniture;
            Setup();
        }

        public void Setup()
        {
            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = cube.gameObject;
            entityMeshWrapper.cellCoordinatesList = furniture.cellCoordinatesList;
            entityMeshWrapper.Setup();

            GetComponent<GameWorldEntity>().Setup();
        }

        public void Teardown()
        {

        }

        /* 
            Static API
         */
        public static GameWorldFurniture Create(Transform parent)
        {
            GameWorldFurnitureManager furnitureManager = GameWorldFurnitureManager.Find();
            GameObject prefab = furnitureManager.prefabAssets.FindByKey(GameWorldFurnitureManager.AssetKey.Furniture);
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;

            GameWorldFurniture gameWorldFurniture = gameObject.GetComponent<GameWorldFurniture>();
            return gameWorldFurniture;
        }
    }
}
