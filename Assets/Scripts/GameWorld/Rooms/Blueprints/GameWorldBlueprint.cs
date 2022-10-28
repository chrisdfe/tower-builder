using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms.Blueprints
{
    public class GameWorldBlueprint : MonoBehaviour
    {
        GameWorldRoom gameWorldRoom;
        List<GameWorldBlueprintCell> gameWorldBlueprintCells = new List<GameWorldBlueprintCell>();

        public static GameWorldBlueprint Create()
        {
            GameObject blueprintPrefab = Resources.Load<GameObject>("Prefabs/Map/Blueprints/Blueprint");
            GameObject blueprintGameObject = GameObject.Instantiate<GameObject>(blueprintPrefab);
            GameWorldBlueprint blueprint = blueprintGameObject.GetComponent<GameWorldBlueprint>();
            Debug.Log("Blueprint has been created");
            return blueprint;
        }

        public void Initialize()
        {
            CreateBlueprintRoom();
            CreateBlueprintCells();
        }

        public void ResetBlueprintCells()
        {
            DestroyBlueprintCells();
            CreateBlueprintCells();
        }

        public void ResetBlueprintRoom()
        {
            DestroyBlueprintRoom();
            CreateBlueprintRoom();
        }

        void Awake()
        {
            Debug.Log("Blueprint has been awoken");
            CreateBlueprintRoom();
            CreateBlueprintCells();
        }

        void OnDestroy()
        {
            DestroyBlueprintRoom();
            DestroyBlueprintCells();
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            ResetBlueprintRoom();
            ResetBlueprintCells();
        }

        void OnSelectedRoomTemplateUpdated(RoomTemplate selectedRoomTemplate)
        {
            // Debug.Log("on selected room template updated");
            // Debug.Log(Registry.appState.UI.buildToolSubState.currentBlueprint);
            ResetBlueprintRoom();
            ResetBlueprintCells();
        }

        void OnToolStateChanged(ToolState toolState, ToolState previousToolState)
        {
            // if (toolState == ToolState.Build) {

            // }
            // Debug.Log("OnToolStateChanged");
            // Debug.Log(Registry.appState.UI.buildToolSubState.currentBlueprint);
            // ResetBlueprintCells();
        }

        void CreateBlueprintRoom()
        {
            Blueprint blueprint = Registry.appState.UI.buildToolSubState.currentBlueprint;
            gameWorldRoom = GameWorldRoom.Create(transform);
            // Debug.Log("gameworldRoom");
            // Debug.Log(gameWorldRoom);
            gameWorldRoom.SetRoom(blueprint.room);
            gameWorldRoom.Initialize();
        }

        void DestroyBlueprintRoom()
        {
            Destroy(gameWorldRoom.gameObject);
            gameWorldRoom = null;
        }

        void CreateBlueprintCells()
        {
            Blueprint blueprint = Registry.appState.UI.buildToolSubState.currentBlueprint;

            foreach (BlueprintCell blueprintCell in blueprint.roomBlueprintCells)
            {
                GameWorldBlueprintCell gameWorldBlueprintCell = GameWorldBlueprintCell.Create(transform);

                gameWorldBlueprintCell.parentBlueprint = this;
                gameWorldBlueprintCell.blueprintCell = blueprintCell;

                gameWorldBlueprintCell.gameWorldRoomCell = FindGameWorldRoomCellForBlueprintCell(gameWorldBlueprintCell);

                gameWorldBlueprintCell.Initialize();

                gameWorldBlueprintCells.Add(gameWorldBlueprintCell);
            }
        }

        GameWorldRoomCell FindGameWorldRoomCellForBlueprintCell(GameWorldBlueprintCell gameWorldBlueprintCell)
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoom.gameWorldRoomCells)
            {
                if (gameWorldRoomCell.roomCell == gameWorldBlueprintCell.blueprintCell.roomCell)
                {
                    return gameWorldRoomCell;
                }
            }

            return null;
        }

        void DestroyBlueprintCells()
        {
            foreach (GameWorldBlueprintCell mapRoomBlueprintCell in gameWorldBlueprintCells)
            {
                Destroy(mapRoomBlueprintCell.gameObject);
            }

            gameWorldBlueprintCells = new List<GameWorldBlueprintCell>();
        }
    }
}
