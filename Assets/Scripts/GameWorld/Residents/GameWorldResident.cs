using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.GameWorld.Residents
{
    public class GameWorldResident : MonoBehaviour
    {
        public Resident resident;

        Transform cube;

        void Awake()
        {
            cube = transform.Find("Cube");
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            UpdatePosition();
        }

        public void Teardown() { }

        void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(resident.cellCoordinates);
        }

        /* 
            Static API
         */
        public static GameWorldResident Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Residents/Resident");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;

            GameWorldResident gameWorldResident = gameObject.GetComponent<GameWorldResident>();
            return gameWorldResident;
        }
    }
}
