using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
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

            Registry.appState.Residents.events.onResidentPositionUpdated += OnResidentPositionUpdated;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void OnDestroy()
        {
            Registry.appState.Residents.events.onResidentsAdded -= OnResidentsAdded;
            Registry.appState.Residents.events.onResidentsRemoved -= OnResidentsRemoved;

            Registry.appState.Residents.events.onResidentPositionUpdated -= OnResidentPositionUpdated;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /*
            Event handlers
        */
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

        void OnResidentPositionUpdated(Resident resident)
        {
            GameWorldResident gameWorldResident = gameWorldResidentsList.Find(gameWorldResident => gameWorldResident.resident == resident);
            gameWorldResident.UpdatePosition();
        }

        void OnCurrentSelectedEntityUpdated(EntityBase entity)
        {
            foreach (GameWorldResident gameWorldResident in gameWorldResidentsList)
            {
                if ((entity is ResidentEntity) && ((ResidentEntity)entity).resident == gameWorldResident.resident)
                {
                    gameWorldResident.SetInspectedColor();
                }
                else
                {
                    gameWorldResident.SetDefaultColor();
                }
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
