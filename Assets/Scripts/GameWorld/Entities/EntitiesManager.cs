using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.Entities.Floors;
using TowerBuilder.GameWorld.Entities.Foundations;
using TowerBuilder.GameWorld.Entities.Freight;
using TowerBuilder.GameWorld.Entities.Furnitures;
using TowerBuilder.GameWorld.Entities.InteriorLights;
using TowerBuilder.GameWorld.Entities.Residents;
using TowerBuilder.GameWorld.Entities.TransportationItems;
using TowerBuilder.GameWorld.Entities.Wheels;
using TowerBuilder.GameWorld.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class EntitiesManager : MonoBehaviour
    {
        public Dictionary<Type, GameWorldEntityList> entityManagerMap { get; private set; }
        public List<GameWorldEntityList> entityManagerList { get; private set; }

        void Awake()
        {
            entityManagerMap = new Dictionary<Type, GameWorldEntityList>()
            {
                {
                    typeof(DataTypes.Entities.Foundations.Foundation),
                    GameWorldFoundationsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Floors.Floor),
                    GameWorldFloorsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.InteriorLights.InteriorLight),
                    GameWorldInteriorLightsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Residents.Resident),
                    GameWorldResidentsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Furnitures.Furniture),
                    GameWorldFurnitureManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.TransportationItems.TransportationItem),
                    GameWorldTransportationManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Freights.FreightItem),
                    GameWorldFreightManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Wheels.Wheel),
                    GameWorldWheelsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Windows.Window),
                    GameWorldWindowsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                }
            };

            entityManagerList = entityManagerMap.Values.ToList();

            Setup();
        }

        public void Setup()
        {
            Registry.appState.Entities.onEntitiesAdded += OnEntitiesAdded;
            Registry.appState.Entities.onEntitiesRemoved += OnEntitiesRemoved;
            Registry.appState.Entities.onEntitiesBuilt += OnEntitiesBuilt;

            Registry.appState.Tools.Destroy.onDestroySelectionUpdated += OnDestroySelectionUpdated;
            Registry.appState.Tools.Inspect.onInspectedEntityListUpdated += OnInspectedEntityListUpdated;
            Registry.appState.Tools.Inspect.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Entities.onEntitiesAdded -= OnEntitiesAdded;
            Registry.appState.Entities.onEntitiesRemoved -= OnEntitiesRemoved;
            Registry.appState.Entities.onEntitiesBuilt -= OnEntitiesBuilt;

            Registry.appState.Tools.Destroy.onDestroySelectionUpdated -= OnDestroySelectionUpdated;
            Registry.appState.Tools.Inspect.onInspectedEntityListUpdated -= OnInspectedEntityListUpdated;
            Registry.appState.Tools.Inspect.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        void OnEntitiesAdded(ListWrapper<Entity> entityList)
        {
            entityList.items.ForEach((entity) =>
            {
                GetListByType(entity.GetType())?.CreateEntity(entity);
            });
        }

        void OnEntitiesRemoved(ListWrapper<Entity> entityList)
        {
            entityList.items.ForEach((entity) =>
            {
                GetListByType(entity.GetType())?.RemoveEntity(entity);
            });
        }

        void OnEntitiesBuilt(ListWrapper<Entity> entityList)
        {
            entityList.items.ForEach((entity) =>
            {
                GetListByType(entity.GetType())?.BuildEntity(entity);
            });
        }

        void OnInspectedEntityListUpdated(ListWrapper<Entity> entitiesList)
        {
            foreach (GameWorldEntityList entityList in entityManagerList)
            {
                entityList.UpdateEntityColors();
            }
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            foreach (GameWorldEntityList entityList in entityManagerList)
            {
                entityList.UpdateEntityColors();
            }
        }

        void OnDestroySelectionUpdated()
        {
            foreach (GameWorldEntityList entityList in entityManagerList)
            {
                entityList.UpdateEntityColors();
            }
        }

        GameWorldEntityList GetListByType(Type EntityType) => entityManagerMap[EntityType];
    }
}