using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.GameWorld.Residents
{
    public class GameWorldResidentsManager : MonoBehaviour
    {
        GameObject gameWorldResidentPrefab;
        List<GameWorldResident> gameWorldResidents = new List<GameWorldResident>();

        public void Awake()
        {
            gameWorldResidentPrefab = Resources.Load<GameObject>("Prefabs/Residents/Resident");

            Registry.appState.Residents.onResidentAdded += OnResidentAdded;
            Registry.appState.Residents.onResidentRemoved += OnResidentDestroyed;
        }

        void OnResidentAdded(Resident resident)
        {
            CreateGameWorldResident(resident);
        }

        void OnResidentDestroyed(Resident resident)
        {
            foreach (GameWorldResident gameWorldResident in gameWorldResidents)
            {
                if (gameWorldResident.resident == resident)
                {
                    gameWorldResidents.Remove(gameWorldResident);
                    Destroy(gameWorldResident.gameObject);
                }
            }
        }

        void CreateGameWorldResident(Resident resident)
        {
            GameObject gameWorldResidentGameObject = Instantiate(gameWorldResidentPrefab);
            GameWorldResident gameWorldResident = gameWorldResidentGameObject.GetComponent<GameWorldResident>();
            gameWorldResident.transform.parent = transform;
            gameWorldResident.resident = resident;
        }
    }
}
