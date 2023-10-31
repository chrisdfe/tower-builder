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
        public Dictionary<Type, EntityTypeManagerBase> entityManagerMap { get; private set; }
        public List<EntityTypeManagerBase> entityManagerList { get; private set; }

        public void Awake()
        {
            entityManagerMap = new Dictionary<Type, EntityTypeManagerBase>()
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
            Registry.appState.Entities.onItemsAdded += OnEntitiesAdded;
            Registry.appState.Entities.onItemsRemoved += OnEntitiesRemoved;
            Registry.appState.Entities.onItemsBuilt += OnEntitiesBuilt;
        }

        public void Teardown()
        {
            Registry.appState.Entities.onItemsAdded -= OnEntitiesAdded;
            Registry.appState.Entities.onItemsRemoved -= OnEntitiesRemoved;
            Registry.appState.Entities.onItemsBuilt -= OnEntitiesBuilt;
        }

        /*
            Event Handlers
        */
        void OnEntitiesAdded(ListWrapper<Entity> entityList)
        {
            entityList.items.ForEach((entity) =>
            {
                GetManagerForEntity(entity)?.CreateEntity(entity);
            });
        }

        void OnEntitiesRemoved(ListWrapper<Entity> entityList)
        {
            entityList.items.ForEach((entity) =>
            {
                GetManagerForEntity(entity)?.RemoveEntity(entity);
            });
        }

        void OnEntitiesBuilt(ListWrapper<Entity> entityList)
        {
            entityList.items.ForEach((entity) =>
            {
                GetManagerForEntity(entity)?.BuildEntity(entity);
            });
        }

        EntityTypeManagerBase GetManagerForEntityType(Type EntityType) => entityManagerMap[EntityType];

        EntityTypeManagerBase GetManagerForEntity(Entity entity)
        {
            return GetManagerForEntityType(entity.GetType());
        }
    }
}