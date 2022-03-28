using System.Collections;
using System.Collections.Generic;
using TowerBuilder.GameWorld.Map.Rooms;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Blueprints
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

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnDestroy()
        {
            DestroyBlueprintRoom();
            DestroyBlueprintCells();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated -= OnSelectedRoomKeyUpdated;
        }

        void CreateBlueprintRoom()
        {
            Blueprint blueprint = Registry.Stores.MapUI.buildToolSubState.currentBlueprint;
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
            Blueprint blueprint = Registry.Stores.MapUI.buildToolSubState.currentBlueprint;

            foreach (BlueprintCell blueprintCell in blueprint.roomBlueprintCells)
            {
                GameObject gameWorldBlueprintCellGameObject = Instantiate<GameObject>(gameWorldBlueprintCellPrefab);
                GameWorldBlueprintCell gameWorldBlueprintCell = gameWorldBlueprintCellGameObject.GetComponent<GameWorldBlueprintCell>();

                gameWorldBlueprintCell.transform.SetParent(transform);
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


        void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            ResetBlueprintRoom();
            ResetBlueprintCells();
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            ResetBlueprintRoom();
            ResetBlueprintCells();
        }
    }
}
