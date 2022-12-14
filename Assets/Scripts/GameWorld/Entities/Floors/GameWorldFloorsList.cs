using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    public class GameWorldFloorsList : MonoBehaviour
    {
        List<GameWorldFloor> gameWorldFloorsList = new List<GameWorldFloor>();

        public void Awake()
        {
            Registry.appState.Entities.Floors.events.onItemsAdded += OnFloorsAdded;
            Registry.appState.Entities.Floors.events.onItemsRemoved += OnFloorsRemoved;
        }

        public void OnDestroy()
        {
            Registry.appState.Entities.Floors.events.onItemsAdded -= OnFloorsAdded;
            Registry.appState.Entities.Floors.events.onItemsRemoved -= OnFloorsRemoved;
        }

        /* 
            Internals
        */
        void AddFloor(Floor floor)
        {
            GameWorldFloor gameWorldFloor = CreateGameWorldFloor(floor);
            gameWorldFloorsList.Add(gameWorldFloor);
            gameWorldFloor.Setup();
        }

        void RemoveFloor(Floor floor)
        {
            GameWorldFloor gameWorldFloorToRemove = gameWorldFloorsList.Find(gameWorldFloor => gameWorldFloor.floor == floor);

            if (gameWorldFloorToRemove != null)
            {
                gameWorldFloorToRemove.Teardown();
                GameObject.Destroy(gameWorldFloorToRemove.gameObject);
                gameWorldFloorsList.Remove(gameWorldFloorToRemove);
            }
        }

        void SetFloorColor(GameWorldFloor gameWorldFloor)
        {
            if (gameWorldFloor.floor.isInBlueprintMode)
            {
                gameWorldFloor.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.ValidBlueprint);
            }
            else
            {
                gameWorldFloor.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
            }
        }

        GameWorldFloor FindGameWorldFloorByFloor(Floor floor) =>
            gameWorldFloorsList.Find(gameWorldFloor => gameWorldFloor.floor == floor);

        /*
            Event handlers
        */
        void OnFloorsAdded(ListWrapper<Floor> floorList)
        {
            foreach (Floor floor in floorList.items)
            {
                AddFloor(floor);
            }
        }

        void OnFloorsRemoved(ListWrapper<Floor> floorList)
        {
            foreach (Floor floor in floorList.items)
            {
                RemoveFloor(floor);
            }
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            foreach (GameWorldFloor gameWorldFloor in gameWorldFloorsList)
            {
                if ((entity is Floor) && ((Floor)entity) == gameWorldFloor.floor)
                {
                    gameWorldFloor.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Inspected);
                }
                else
                {
                    gameWorldFloor.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
                }
            }
        }

        /*
            Static API
        */
        GameWorldFloor CreateGameWorldFloor(Floor floor)
        {
            GameWorldFloor gameWorldFloor = GameWorldFloor.Create(transform);

            gameWorldFloor.floor = floor;
            gameWorldFloor.Setup();

            return gameWorldFloor;
        }
    }
}
