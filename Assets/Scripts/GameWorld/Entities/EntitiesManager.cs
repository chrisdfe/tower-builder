using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
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
        public Dictionary<Type, EntityTypeManager> entityManagerMap { get; private set; }
        public List<EntityTypeManager> entityManagerList { get; private set; }

        void Awake()
        {
            entityManagerMap = new Dictionary<Type, EntityTypeManager>()
            {
                {
                    typeof(DataTypes.Entities.Foundations.Foundation),
                    GameWorldFoundationsManager.Find()
                },
                {
                    typeof(DataTypes.Entities.Floors.Floor),
                    GameWorldFloorsManager.Find()
                },
                {
                    typeof(DataTypes.Entities.InteriorLights.InteriorLight),
                    GameWorldInteriorLightsManager.Find()
                },
                {
                    typeof(DataTypes.Entities.Residents.Resident),
                    GameWorldResidentsManager.Find()
                },
                {
                    typeof(DataTypes.Entities.Furnitures.Furniture),
                    GameWorldFurnitureManager.Find()
                },
                {
                    typeof(DataTypes.Entities.TransportationItems.TransportationItem),
                    GameWorldTransportationManager.Find()
                },
                {
                    typeof(DataTypes.Entities.Freights.FreightItem),
                    GameWorldFreightManager.Find()
                },
                {
                    typeof(DataTypes.Entities.Wheels.Wheel),
                    GameWorldWheelsManager.Find()
                },
                {
                    typeof(DataTypes.Entities.Windows.Window),
                    GameWorldWindowsManager.Find()
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

            Registry.appState.Entities.onEntityPositionUpdated += OnEntityPositionUpdated;

            Registry.appState.EntityGroups.onPositionUpdated += OnEntityGroupPositionUpdated;

            Registry.appState.Tools.Destroy.onDestroySelectionUpdated += OnDestroySelectionUpdated;
            Registry.appState.Tools.Inspect.onInspectedEntityListUpdated += OnInspectedEntityListUpdated;
            Registry.appState.Tools.Inspect.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Entities.onEntitiesAdded -= OnEntitiesAdded;
            Registry.appState.Entities.onEntitiesRemoved -= OnEntitiesRemoved;
            Registry.appState.Entities.onEntitiesBuilt -= OnEntitiesBuilt;

            Registry.appState.Entities.onEntityPositionUpdated -= OnEntityPositionUpdated;

            Registry.appState.EntityGroups.onPositionUpdated -= OnEntityGroupPositionUpdated;

            Registry.appState.Tools.Destroy.onDestroySelectionUpdated -= OnDestroySelectionUpdated;
            Registry.appState.Tools.Inspect.onInspectedEntityListUpdated -= OnInspectedEntityListUpdated;
            Registry.appState.Tools.Inspect.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /*
            Event Handlers
        */
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

        void OnEntityPositionUpdated(Entity entity)
        {
            GetListByType(entity.GetType())?.UpdateEntityPosition(entity);
            GetListByType(entity.GetType())?.UpdateEntityColor(entity);
        }

        void OnEntityGroupPositionUpdated(EntityGroup entityGroup)
        {
            foreach (Entity entity in entityGroup.entities.items)
            {
                GetListByType(entity.GetType())?.UpdateEntityPosition(entity);
                GetListByType(entity.GetType())?.UpdateEntityColor(entity);
            }
        }

        void OnInspectedEntityListUpdated(ListWrapper<Entity> entitiesList)
        {
            foreach (EntityTypeManager entityList in entityManagerList)
            {
                entityList.UpdateAllEntityColors();
            }
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            Debug.Log("on current selected entity updated");
            Debug.Log(entity);

            foreach (EntityTypeManager entityList in entityManagerList)
            {
                entityList.UpdateAllEntityColors();
            }
        }

        void OnDestroySelectionUpdated()
        {
            foreach (EntityTypeManager entityList in entityManagerList)
            {
                entityList.UpdateAllEntityColors();
            }
        }

        EntityTypeManager GetListByType(Type EntityType) => entityManagerMap[EntityType];
    }
}