using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Wheels
{
    public class GameWorldWheelsList : MonoBehaviour
    {
        List<GameWorldWheel> gameWorldWheelsList = new List<GameWorldWheel>();

        public void Awake()
        {
            Registry.appState.Entities.Wheels.events.onItemsAdded += OnWheelsAdded;
            Registry.appState.Entities.Wheels.events.onItemsRemoved += OnWheelsRemoved;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void OnDestroy()
        {
            Registry.appState.Entities.Wheels.events.onItemsAdded -= OnWheelsAdded;
            Registry.appState.Entities.Wheels.events.onItemsRemoved -= OnWheelsRemoved;

            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /* 
            Internals
        */
        void AddWheel(Wheel wheel)
        {
            GameWorldWheel gameWorldWheel = CreateGameWorldWheel(wheel);
            gameWorldWheelsList.Add(gameWorldWheel);

            SetWheelColor(gameWorldWheel);
        }

        void RemoveWheel(Wheel wheel)
        {
            GameWorldWheel gameWorldWheelToRemove = gameWorldWheelsList.Find(gameWorldWheel => gameWorldWheel.wheel == wheel);

            if (gameWorldWheelToRemove != null)
            {
                GameObject.Destroy(gameWorldWheelToRemove.gameObject);
                gameWorldWheelsList.Remove(gameWorldWheelToRemove);
            }
        }

        void SetWheelColor(GameWorldWheel gameWorldWheel)
        {
            if (gameWorldWheel.wheel.isInBlueprintMode)
            {
                gameWorldWheel.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.ValidBlueprint);
            }
            else
            {
                gameWorldWheel.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
            }
        }

        GameWorldWheel FindGameWorldWheelByWheel(Wheel wheel)
        {
            return gameWorldWheelsList.Find(gameWorldWheel => gameWorldWheel.wheel == wheel);
        }

        /*
            Event handlers
        */
        void OnWheelsAdded(ListWrapper<Wheel> wheelList)
        {
            foreach (Wheel wheel in wheelList.items)
            {
                AddWheel(wheel);
            }
        }

        void OnWheelsRemoved(ListWrapper<Wheel> wheelList)
        {
            foreach (Wheel wheel in wheelList.items)
            {
                RemoveWheel(wheel);
            }
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            foreach (GameWorldWheel gameWorldWheel in gameWorldWheelsList)
            {
                if ((entity is Wheel) && ((Wheel)entity) == gameWorldWheel.wheel)
                {
                    gameWorldWheel.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Inspected);
                }
                else
                {
                    gameWorldWheel.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
                }
            }
        }

        /*
            Static API
        */
        GameWorldWheel CreateGameWorldWheel(Wheel wheel)
        {
            GameWorldWheel gameWorldWheel = GameWorldWheel.Create(transform);

            gameWorldWheel.wheel = wheel;
            gameWorldWheel.Setup();

            return gameWorldWheel;
        }
    }
}
