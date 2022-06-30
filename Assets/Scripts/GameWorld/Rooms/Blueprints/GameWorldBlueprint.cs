using System.Collections;
using System.Collections.Generic;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.Rooms.Blueprints;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms.Blueprints
{
    public class GameWorldBlueprint : MonoBehaviour
    {
        GameObject gameWorldRoomPrefab;
        GameObject gameWorldBlueprintCellPrefab;

        GameWorldRoom gameWorldRoom;
        List<GameWorldBlueprintCell> gameWorldBlueprintCells = new List<GameWorldBlueprintCell>();

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
            gameWorldBlueprintCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Blueprints/BlueprintCell");
            gameWorldRoomPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/Room");

            CreateBlueprintRoom();
            CreateBlueprintCells();

            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.appState.UI.buildToolSubState.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        void OnDestroy()
        {
            DestroyBlueprintRoom();
            DestroyBlueprintCells();

            Registry.appState.UI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
            Registry.appState.UI.buildToolSubState.onSelectedRoomTemplateUpdated -= OnSelectedRoomTemplateUpdated;
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            ResetBlueprintRoom();
            ResetBlueprintCells();
        }

        void OnSelectedRoomTemplateUpdated(RoomTemplate selectedRoomTemplate)
        {
            ResetBlueprintRoom();
            ResetBlueprintCells();
        }

        void CreateBlueprintRoom()
        {
            Blueprint blueprint = Registry.appState.UI.buildToolSubState.currentBlueprint;
            GameObject gameWorldRoomGameObject = Instantiate(gameWorldRoomPrefab);
            gameWorldRoom = gameWorldRoomGameObject.GetComponent<GameWorldRoom>();
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
                GameObject gameWorldBlueprintCellGameObject = Instantiate<GameObject>(gameWorldBlueprintCellPrefab, transform);
                GameWorldBlueprintCell gameWorldBlueprintCell = gameWorldBlueprintCellGameObject.GetComponent<GameWorldBlueprintCell>();

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
