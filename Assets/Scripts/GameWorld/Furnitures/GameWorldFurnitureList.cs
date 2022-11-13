using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Furnitures;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.GameWorld.Furnitures
{
    public class GameWorldFurnitureList : MonoBehaviour
    {
        List<GameWorldFurniture> gameWorldFurnitureList = new List<GameWorldFurniture>();

        void Awake()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        void Setup()
        {
            Registry.appState.Furnitures.events.onFurnituresAdded += OnFurnituresAdded;
            Registry.appState.Furnitures.events.onFurnituresRemoved += OnFurnituresRemoved;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        void Teardown()
        {
            Registry.appState.Furnitures.events.onFurnituresAdded -= OnFurnituresAdded;
            Registry.appState.Furnitures.events.onFurnituresRemoved -= OnFurnituresRemoved;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        void OnFurnituresAdded(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                AddFurniture(furniture);
            }
        }

        void OnFurnituresRemoved(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                RemoveFurniture(furniture);
            }
        }

        void OnCurrentSelectedEntityUpdated(EntityBase entity)
        {
            foreach (GameWorldFurniture gameWorldFurniture in gameWorldFurnitureList)
            {
                if ((entity is FurnitureEntity) && ((FurnitureEntity)entity).furniture == gameWorldFurniture.furniture)
                {
                    gameWorldFurniture.SetInspectedColor();
                }
                else
                {
                    gameWorldFurniture.SetDefaultColor();
                }
            }
        }

        void AddFurniture(Furniture furniture)
        {
            GameWorldFurniture gameWorldFurniture = CreateGameWorldFurniture(furniture);
            gameWorldFurnitureList.Add(gameWorldFurniture);
        }

        void RemoveFurniture(Furniture furniture)
        {
            GameWorldFurniture gameWorldFurnitureToRemove = gameWorldFurnitureList.Find(gameWorldFurniture => gameWorldFurniture.furniture == furniture);

            if (gameWorldFurnitureToRemove != null)
            {
                GameObject.Destroy(gameWorldFurnitureToRemove.gameObject);
                gameWorldFurnitureList.Remove(gameWorldFurnitureToRemove);
            }
        }

        GameWorldFurniture CreateGameWorldFurniture(Furniture furniture)
        {
            GameWorldFurniture gameWorldFurniture = GameWorldFurniture.Create(transform);
            gameWorldFurniture.furniture = furniture;
            gameWorldFurniture.Setup();
            return gameWorldFurniture;
        }
    }
}
