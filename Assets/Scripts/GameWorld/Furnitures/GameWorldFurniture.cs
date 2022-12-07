using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Furnitures;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Furnitures
{
    public class GameWorldFurniture : MonoBehaviour
    {
        public Furniture furniture { get; set; }

        Transform cube;
        Color defaultColor;

        void Awake()
        {
            cube = transform.Find("Cube");
            defaultColor = cube.GetComponent<MeshRenderer>().material.color;
        }

        public void Setup()
        {
            UpdatePosition();
        }

        public void Teardown() { }

        public void SetDefaultColor()
        {
            SetColor(defaultColor);
        }

        public void SetValidBlueprintColor()
        {
            SetColor(Color.blue);
        }

        public void SetInvalidBlueprintColor()
        {
            SetColor(Color.red);
        }

        public void SetInspectedColor()
        {
            SetColor(Color.cyan);
        }

        public void SetColor(Color color, float alpha = 1.0f)
        {
            Material material = cube.GetComponent<MeshRenderer>().material;
            material.color = new Color(color.r, color.g, color.b, alpha);
        }

        void UpdatePosition()
        {
            Debug.Log("position before");
            Debug.Log(transform.position);
            transform.position = GameWorldUtils.CellCoordinatesToPosition(furniture.cellCoordinatesList.bottomLeftCoordinates);
            Debug.Log("furniture.cellCoordinatesList.bottomLeftCoordinates");
            Debug.Log(furniture.cellCoordinatesList.bottomLeftCoordinates);
            Debug.Log("position after");
            Debug.Log(transform.position);
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
