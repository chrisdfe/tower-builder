using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    public class GameWorldFurniture : MonoBehaviour, IGameWorldEntity
    {
        public Furniture furniture { get; set; }
        public EntityMeshWrapper entityMeshWrapper { get; private set; }

        Transform cube;

        void Awake()
        {
            cube = transform.Find("Placeholder");
        }

        public void Setup()
        {
            entityMeshWrapper = new EntityMeshWrapper(transform, cube.gameObject, furniture.cellCoordinatesList);
            entityMeshWrapper.Setup();
            UpdatePosition();
        }

        public void Teardown()
        {

        }

        void UpdatePosition()
        {
            // transform.position = GameWorldUtils.CellCoordinatesToPosition(furniture.cellCoordinatesList.bottomLeftCoordinates);
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
