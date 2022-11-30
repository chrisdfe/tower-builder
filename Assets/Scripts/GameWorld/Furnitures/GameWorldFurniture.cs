using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
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
            transform.position = GameWorldUtils.CellCoordinatesToPosition(furniture.cellCoordinates);
        }

        /* 
            Static API
         */
        public static GameWorldFurniture Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Furnitures/Furniture");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;

            GameWorldFurniture gameWorldFurniture = gameObject.GetComponent<GameWorldFurniture>();
            return gameWorldFurniture;
        }
    }
}
