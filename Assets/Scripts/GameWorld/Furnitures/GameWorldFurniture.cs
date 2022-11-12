using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Furnitures;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.GameWorld.Furnitures
{
    public class GameWorldFurniture : MonoBehaviour
    {
        public Furniture furniture { get; private set; }

        void Awake()
        {
        }

        public void Setup()
        {
            UpdatePosition();
        }

        public void Teardown()
        {

        }

        public void SetFurniture(Furniture furniture)
        {
            this.furniture = furniture;
        }

        void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(furniture.cellCoordinates);
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
