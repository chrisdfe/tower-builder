using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.Entities.Floors;
using TowerBuilder.GameWorld.Entities.Freight;
using TowerBuilder.GameWorld.Entities.Furnitures;
using TowerBuilder.GameWorld.Entities.InteriorWalls;
using TowerBuilder.GameWorld.Entities.Residents;
using TowerBuilder.GameWorld.Entities.Rooms;
using TowerBuilder.GameWorld.Entities.TransportationItems;
using TowerBuilder.GameWorld.Entities.Wheels;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class EntitiesManager : MonoBehaviour
    {
        public Dictionary<Type, GameWorldEntityList> entityManagerMap { get; private set; }

        void Awake()
        {
            entityManagerMap = new Dictionary<Type, GameWorldEntityList>()
            {
                {
                    typeof(DataTypes.Entities.Rooms.Room),
                    GameWorldRoomsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.Floors.Floor),
                    GameWorldFloorsManager.Find()
                        .GetComponent<GameWorldEntityList>()
                },
                {
                    typeof(DataTypes.Entities.InteriorWalls.InteriorWall),
                    GameWorldInteriorWallsManager.Find()
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
                }
            };

            Setup();
        }

        public void Setup()
        {
            Registry.appState.Entities.events.onEntitiesAdded += OnEntitiesAdded;
            Registry.appState.Entities.events.onEntitiesRemoved += OnEntitiesRemoved;
            Registry.appState.Entities.events.onEntitiesBuilt += OnEntitiesBuilt;
        }

        public void Teardown()
        {
            Registry.appState.Entities.events.onEntitiesAdded -= OnEntitiesAdded;
            Registry.appState.Entities.events.onEntitiesRemoved -= OnEntitiesRemoved;
            Registry.appState.Entities.events.onEntitiesBuilt -= OnEntitiesBuilt;
        }

        void OnEntitiesAdded(ListWrapper<Entity> entityList)
        {
            Debug.Log("entities added");
            entityList.items.ForEach((entity) =>
            {
                GetListByType(entity.GetType())?.CreateEntity(entity);
                Debug.Log(entity.GetType());
            });
        }

        void OnEntitiesRemoved(ListWrapper<Entity> entityList)
        {
            Debug.Log("entities removed");
            entityList.items.ForEach((entity) =>
            {
                GetListByType(entity.GetType())?.RemoveEntity(entity);
                Debug.Log(entity.GetType());
            });
        }

        void OnEntitiesBuilt(ListWrapper<Entity> entityList)
        {
            Debug.Log("entities built");

            entityList.items.ForEach((entity) =>
            {
                GetListByType(entity.GetType())?.BuildEntity(entity);
                Debug.Log(entity.GetType());
            });
        }

        GameWorldEntityList GetListByType(Type EntityType) => entityManagerMap[EntityType];
    }
}