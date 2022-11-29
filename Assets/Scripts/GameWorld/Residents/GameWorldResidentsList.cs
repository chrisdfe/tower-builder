using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
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

            Registry.appState.ResidentBehaviors.events.onResidentBehaviorAdded += OnResidentBehaviorAdded;
            Registry.appState.ResidentBehaviors.events.onResidentBehaviorRemoved += OnResidentBehaviorRemoved;
            Registry.appState.ResidentBehaviors.events.onResidentBehaviorTraveled += OnResidentBehaviorTraveled;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void OnDestroy()
        {
            Registry.appState.Residents.events.onResidentsAdded -= OnResidentsAdded;
            Registry.appState.Residents.events.onResidentsRemoved -= OnResidentsRemoved;

            Registry.appState.Residents.events.onResidentPositionUpdated -= OnResidentPositionUpdated;

            Registry.appState.ResidentBehaviors.events.onResidentBehaviorAdded -= OnResidentBehaviorAdded;
            Registry.appState.ResidentBehaviors.events.onResidentBehaviorRemoved -= OnResidentBehaviorRemoved;
            Registry.appState.ResidentBehaviors.events.onResidentBehaviorTraveled -= OnResidentBehaviorTraveled;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /* 
            Internals
        */
        void AddResident(Resident resident)
        {
            GameWorldResident gameWorldResident = CreateGameWorldResident(resident);
            gameWorldResidentsList.Add(gameWorldResident);
            gameWorldResident.Setup();
            SetResidentColor(gameWorldResident);
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

        void SetResidentColor(GameWorldResident gameWorldResident)
        {
            if (gameWorldResident.resident.isInBlueprintMode)
            {
                gameWorldResident.SetColor(GameWorldResident.ColorKey.ValidBlueprint);
            }
            else
            {
                gameWorldResident.SetColor(GameWorldResident.ColorKey.Default);
            }
        }

        GameWorldResident FindGameWorldResidentByResident(Resident resident)
        {
            return gameWorldResidentsList.Find(gameWorldResident => gameWorldResident.resident == resident);
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
                    gameWorldResident.SetColor(GameWorldResident.ColorKey.Inspected);
                }
                else
                {
                    gameWorldResident.SetColor(GameWorldResident.ColorKey.Default);
                }
            }
        }

        void OnResidentBehaviorAdded(ResidentBehavior residentBehavior)
        {
            GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
            if (gameWorldResident != null)
            {
                gameWorldResident.residentBehavior = residentBehavior;
            }
        }

        void OnResidentBehaviorRemoved(ResidentBehavior residentBehavior)
        {
            GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
            if (gameWorldResident != null)
            {
                gameWorldResident.residentBehavior = null;
            }
        }

        void OnResidentBehaviorTraveled(ResidentBehavior residentBehavior, TravelingStateHandler travelingStateHandler)
        {
            GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
            if (gameWorldResident == null) return;

            // TODO - do this on state change and also other times, not just travel state.
            TimeValue currentTick = Registry.appState.Time.time;
            TimeValue nextTick = TimeValue.Add(currentTick, new TimeValue.Input() { minute = Constants.MINUTES_ELAPSED_PER_TICK });

            gameWorldResident.currentAndNextPosition = new CurrentAndNext<(TimeValue, CellCoordinates)>(
                (currentTick, travelingStateHandler.routeProgress.currentCell),
                (nextTick, travelingStateHandler.routeProgress.nextCell)
            );
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
