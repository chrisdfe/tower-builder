using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.InteriorWalls;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorWalls
{
    public class GameWorldInteriorWallList : MonoBehaviour
    {
        List<GameWorldInteriorWall> gameWorldInteriorWallsList = new List<GameWorldInteriorWall>();

        public void Awake()
        {
            Registry.appState.Entities.InteriorWalls.events.onItemsAdded += OnInteriorWallsAdded;
            Registry.appState.Entities.InteriorWalls.events.onItemsRemoved += OnInteriorWallsRemoved;
        }

        public void OnDestroy()
        {
            Registry.appState.Entities.InteriorWalls.events.onItemsAdded -= OnInteriorWallsAdded;
            Registry.appState.Entities.InteriorWalls.events.onItemsRemoved -= OnInteriorWallsRemoved;
        }

        /* 
            Internals
        */
        void AddInteriorWall(InteriorWall interiorWall)
        {
            GameWorldInteriorWall gameWorldInteriorWall = CreateGameWorldInteriorWall(interiorWall);
            gameWorldInteriorWallsList.Add(gameWorldInteriorWall);
            gameWorldInteriorWall.Setup();
        }

        void RemoveInteriorWall(InteriorWall interiorWall)
        {
            GameWorldInteriorWall gameWorldInteriorWallToRemove = gameWorldInteriorWallsList.Find(gameWorldInteriorWall => gameWorldInteriorWall.interiorWall == interiorWall);

            if (gameWorldInteriorWallToRemove != null)
            {
                gameWorldInteriorWallToRemove.Teardown();
                GameObject.Destroy(gameWorldInteriorWallToRemove.gameObject);
                gameWorldInteriorWallsList.Remove(gameWorldInteriorWallToRemove);
            }
        }

        void SetInteriorWallColor(GameWorldInteriorWall gameWorldInteriorWall)
        {
            if (gameWorldInteriorWall.interiorWall.isInBlueprintMode)
            {
                gameWorldInteriorWall.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.ValidBlueprint);
            }
            else
            {
                gameWorldInteriorWall.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
            }
        }

        GameWorldInteriorWall FindGameWorldInteriorWallByInteriorWall(InteriorWall interiorWall) =>
            gameWorldInteriorWallsList.Find(gameWorldInteriorWall => gameWorldInteriorWall.interiorWall == interiorWall);

        /*
            Event handlers
        */
        void OnInteriorWallsAdded(ListWrapper<InteriorWall> interiorWallList)
        {
            Debug.Log("interior walls added");
            foreach (InteriorWall interiorWall in interiorWallList.items)
            {
                AddInteriorWall(interiorWall);
            }
        }

        void OnInteriorWallsRemoved(ListWrapper<InteriorWall> interiorWallList)
        {
            Debug.Log("interior walls removed");
            foreach (InteriorWall interiorWall in interiorWallList.items)
            {
                RemoveInteriorWall(interiorWall);
            }
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            foreach (GameWorldInteriorWall gameWorldInteriorWall in gameWorldInteriorWallsList)
            {
                if ((entity is InteriorWall) && ((InteriorWall)entity) == gameWorldInteriorWall.interiorWall)
                {
                    gameWorldInteriorWall.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Inspected);
                }
                else
                {
                    gameWorldInteriorWall.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
                }
            }
        }

        /*
            Static API
        */
        GameWorldInteriorWall CreateGameWorldInteriorWall(InteriorWall interiorWall)
        {
            GameWorldInteriorWall gameWorldInteriorWall = GameWorldInteriorWall.Create(transform);

            gameWorldInteriorWall.interiorWall = interiorWall;
            gameWorldInteriorWall.Setup();

            return gameWorldInteriorWall;
        }
    }
}
