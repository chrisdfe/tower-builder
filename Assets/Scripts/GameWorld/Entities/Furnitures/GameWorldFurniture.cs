using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldFurniture : MonoBehaviour
    {
        public Furniture furniture { get; set; }

        EntityMeshWrapper entityMeshWrapper;

        Transform cube;

        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        public void Setup()
        {
            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = cube.gameObject;
            entityMeshWrapper.cellCoordinatesList = furniture.cellCoordinatesList;
            entityMeshWrapper.Setup();
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
