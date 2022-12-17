using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    public class GameWorldResidentsList : MonoBehaviour
    {
        List<GameWorldResident> gameWorldResidentsList = new List<GameWorldResident>();

        public void Awake()
        {
            Registry.appState.Entities.Residents.events.onItemsAdded += OnResidentsAdded;
            Registry.appState.Entities.Residents.events.onItemsRemoved += OnResidentsRemoved;

            Registry.appState.Entities.Residents.events.onItemPositionUpdated += OnResidentPositionUpdated;

            Registry.appState.ResidentBehaviors.events.onItemsAdded += OnResidentBehaviorsAdded;
            Registry.appState.ResidentBehaviors.events.onItemsRemoved += OnResidentBehaviorsRemoved;

            Registry.appState.ResidentBehaviors.events.onTickProcessed += OnResidentBehaviorTickProcessed;
            Registry.appState.ResidentBehaviors.events.onResidentBehaviorStateChanged += OnResidentBehaviorStateChanged;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void OnDestroy()
        {
            Registry.appState.Entities.Residents.events.onItemsAdded -= OnResidentsAdded;
            Registry.appState.Entities.Residents.events.onItemsRemoved -= OnResidentsRemoved;

            Registry.appState.Entities.Residents.events.onItemPositionUpdated -= OnResidentPositionUpdated;

            Registry.appState.ResidentBehaviors.events.onItemsAdded -= OnResidentBehaviorsAdded;
            Registry.appState.ResidentBehaviors.events.onItemsRemoved -= OnResidentBehaviorsRemoved;
            Registry.appState.ResidentBehaviors.events.onTickProcessed -= OnResidentBehaviorTickProcessed;
            Registry.appState.ResidentBehaviors.events.onResidentBehaviorStateChanged -= OnResidentBehaviorStateChanged;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /* 
            Internals
        */
        void AddResident(Resident resident)
        {
            GameWorldResident gameWorldResident = CreateGameWorldResident(resident);
            gameWorldResidentsList.Add(gameWorldResident);

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
                gameWorldResident.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.ValidBlueprint);
            }
            else
            {
                gameWorldResident.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
            }
        }

        GameWorldResident FindGameWorldResidentByResident(Resident resident)
        {
            return gameWorldResidentsList.Find(gameWorldResident => gameWorldResident.resident == resident);
        }

        /*
            Event handlers
        */
        void OnResidentsAdded(ListWrapper<Resident> residentList)
        {
            foreach (Resident resident in residentList.items)
            {
                AddResident(resident);
            }
        }

        void OnResidentsRemoved(ListWrapper<Resident> residentList)
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

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            foreach (GameWorldResident gameWorldResident in gameWorldResidentsList)
            {
                if ((entity is Resident) && ((Resident)entity) == gameWorldResident.resident)
                {
                    gameWorldResident.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Inspected);
                }
                else
                {
                    gameWorldResident.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
                }
            }
        }

        void OnResidentBehaviorsAdded(ResidentBehaviorsList newResidentBehaviors)
        {
            newResidentBehaviors.ForEach(residentBehavior =>
            {
                GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
                if (gameWorldResident != null)
                {
                    gameWorldResident.residentBehavior = residentBehavior;
                }
            });
        }

        void OnResidentBehaviorsRemoved(ResidentBehaviorsList removedResidentBehaviors)
        {
            removedResidentBehaviors.ForEach(residentBehavior =>
            {
                GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
                if (gameWorldResident != null)
                {
                    gameWorldResident.residentBehavior = null;
                }
            });
        }

        void OnResidentBehaviorTickProcessed(ResidentBehavior residentBehavior)
        {
            if (residentBehavior.currentState == ResidentBehavior.StateKey.Traveling)
            {
                GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
                if (gameWorldResident == null) return;

                // TODO - do this on state change and also other times, not just travel state.
                TimeValue currentTick = Registry.appState.Time.time;
                TimeValue nextTick = TimeValue.Add(currentTick, new TimeValue.Input() { minute = Constants.MINUTES_ELAPSED_PER_TICK });

                gameWorldResident.currentAndNextPosition = new CurrentAndNext<(TimeValue, CellCoordinates)>(
                    (currentTick, residentBehavior.routeProgress.currentCell),
                    (nextTick, residentBehavior.routeProgress.nextCell)
                );
            }
        }

        void OnResidentBehaviorStateChanged(ResidentBehavior residentBehavior, ResidentBehavior.StateKey previousState, ResidentBehavior.StateKey currentState)
        {
            if (currentState == ResidentBehavior.StateKey.Traveling)
            {
                GameWorldResident gameWorldResident = FindGameWorldResidentByResident(residentBehavior.resident);
                if (gameWorldResident == null) return;

                // TODO - do this on state change and also other times, not just travel state.
                TimeValue currentTick = Registry.appState.Time.time;
                TimeValue nextTick = TimeValue.Add(currentTick, new TimeValue.Input() { minute = Constants.MINUTES_ELAPSED_PER_TICK });

                gameWorldResident.currentAndNextPosition = new CurrentAndNext<(TimeValue, CellCoordinates)>(
                    (currentTick, residentBehavior.routeProgress.currentCell),
                    (nextTick, residentBehavior.routeProgress.nextCell)
                );
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
