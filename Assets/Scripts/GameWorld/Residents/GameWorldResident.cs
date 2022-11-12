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

        public void Initialize()
        {
            UpdatePosition();
        }

        void Awake()
        {
        }

        void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(resident.coordinates);
        }

        /* 
            Static API
         */
        public static GameWorldResident Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Map/Residents/Resident");
            GameObject gameObject = Instantiate<GameObject>(prefab);

            gameObject.transform.parent = parent;

            GameWorldResident gameWorldResident = gameObject.GetComponent<GameWorldResident>();
            return gameWorldResident;
        }
    }
}
