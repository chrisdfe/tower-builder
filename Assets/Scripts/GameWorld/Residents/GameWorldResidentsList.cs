using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.GameWorld.Residents
{
    public class GameWorldResidentsList : MonoBehaviour
    {
        List<GameWorldResident> gameWorldResidentsList = new List<GameWorldResident>();

        public void Awake()
        {
            Registry.appState.Residents.events.onResidentsAdded += OnResidentsAdded;
            Registry.appState.Residents.events.onResidentsRemoved += OnResidentsRemoved;
        }

        void OnResidentsAdded(ResidentsList residentList)
        {
            foreach (Resident resident in residentList.items)
            {
                AddResident(resident);
            }
        }

        void OnResidentsRemoved(ResidentsList residentList)
        {
            foreach (Resident resident in residentList.items)
            {
                RemoveResident(resident);
            }
        }

        void AddResident(Resident resident)
        {
            GameWorldResident gameWorldResident = CreateGameWorldResident(resident);
            gameWorldResidentsList.Add(gameWorldResident);
        }

        void RemoveResident(Resident resident)
        {
            GameWorldResident gameWorldResidentToRemove = gameWorldResidentsList.Find(gameWorldResident => gameWorldResident.resident == resident);

            if (gameWorldResidentToRemove != null)
            {
                GameObject.Destroy(gameWorldResidentToRemove.gameObject);
                gameWorldResidentsList.Remove(gameWorldResidentToRemove);
            }
        }

        /*
            Static API
        */
        GameWorldResident CreateGameWorldResident(Resident resident)
        {
            GameWorldResident gameWorldResident = GameWorldResident.Create(transform);

            gameWorldResident.resident = resident;
            gameWorldResident.Setup();

            return gameWorldResident;
        }
    }
}
