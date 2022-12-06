using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Furnitures;
using UnityEngine;

namespace TowerBuilder.GameWorld.Furnitures
{
    public class GameWorldFurnitureList : MonoBehaviour
    {
        List<GameWorldFurniture> gameWorldFurnitureList = new List<GameWorldFurniture>();

        /* 
            Lifecycle Methods
        */
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
            Registry.appState.Furnitures.events.onItemsAdded += OnFurnituresAdded;
            Registry.appState.Furnitures.events.onItemsRemoved += OnFurnituresRemoved;

            Registry.appState.UI.events.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        void Teardown()
        {
            Registry.appState.Furnitures.events.onItemsAdded -= OnFurnituresAdded;
            Registry.appState.Furnitures.events.onItemsRemoved -= OnFurnituresRemoved;

            Registry.appState.UI.events.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /*
            Event Handlers
        */
        void OnFurnituresAdded(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                CreateFurniture(furniture);
            }
        }

        void OnFurnituresRemoved(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                RemoveFurniture(furniture);
            }
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            UpdateFurnituresColors();
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            UpdateFurnituresColors();
        }

        /*
            Internals
        */
        void CreateFurniture(Furniture furniture)
        {
            GameWorldFurniture gameWorldFurniture = CreateGameWorldFurniture(furniture);
            gameWorldFurniture.Setup();
            UpdateFurnitureColor(gameWorldFurniture);
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

        void UpdateFurnituresColors()
        {
            foreach (GameWorldFurniture gameWorldFurniture in gameWorldFurnitureList)
            {
                UpdateFurnitureColor(gameWorldFurniture);
            }
        }

        void UpdateFurnitureColor(GameWorldFurniture gameWorldFurniture)
        {
            Entity inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;
            Furniture furniture = gameWorldFurniture.furniture;
            ToolState toolState = Registry.appState.Tools.toolState;

            bool hasUpdated = false;

            switch (toolState)
            {
                case (ToolState.Build):
                    SetBuildStateColor();
                    break;
                case (ToolState.Destroy):
                    SetDestroyStateColor();
                    break;
                default:
                    SetInspectStateColor();
                    break;
            }

            if (!hasUpdated)
            {
                gameWorldFurniture.SetDefaultColor();
            }

            void SetBuildStateColor()
            {

                if (furniture.isInBlueprintMode)
                {
                    if (furniture.validator.isValid)
                    {
                        gameWorldFurniture.SetValidBlueprintColor();
                    }
                    else
                    {
                        gameWorldFurniture.SetInvalidBlueprintColor();
                    }

                    hasUpdated = true;
                }
            }

            void SetDestroyStateColor()
            {
                // not supported yet
            }

            void SetInspectStateColor()
            {
                if ((inspectedEntity is FurnitureEntity) && ((FurnitureEntity)inspectedEntity).furniture == furniture)
                {
                    gameWorldFurniture.SetInspectedColor();
                    hasUpdated = true;
                }
            }
        }

        /* 
            Static API
        */
        GameWorldFurniture CreateGameWorldFurniture(Furniture furniture)
        {
            GameWorldFurniture gameWorldFurniture = GameWorldFurniture.Create(transform);
            gameWorldFurniture.furniture = furniture;
            gameWorldFurniture.Setup();
            return gameWorldFurniture;
        }
    }
}
