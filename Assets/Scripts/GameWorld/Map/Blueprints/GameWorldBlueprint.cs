using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Blueprints
{
    public class GameWorldBlueprint : MonoBehaviour
    {
        public Blueprint blueprint { get; private set; }

        GameObject gameWorldBlueprintCellPrefab;

        List<GameWorldBlueprintCell> gameWorldBlueprintCells = new List<GameWorldBlueprintCell>();

        public void ResetBlueprintCells()
        {
            DestroyBlueprintCells();
            CreateBlueprintCells();
        }

        void Awake()
        {
            gameWorldBlueprintCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Blueprints/BlueprintCell");

            CreateBlueprintCells();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnDestroy()
        {
            DestroyBlueprintCells();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated -= OnSelectedRoomKeyUpdated;
        }

        void CreateBlueprintCells()
        {
            Blueprint blueprint = Registry.Stores.MapUI.buildToolSubState.currentBlueprint;

            foreach (BlueprintCell roomBlueprintCell in blueprint.roomBlueprintCells)
            {
                GameObject newMapRoomBlueprintCellGameObject = Instantiate<GameObject>(gameWorldBlueprintCellPrefab);

                GameWorldBlueprintCell newMapRoomBlueprintCell = newMapRoomBlueprintCellGameObject.GetComponent<GameWorldBlueprintCell>();

                newMapRoomBlueprintCell.transform.SetParent(transform);
                newMapRoomBlueprintCell.parentBlueprint = this;
                newMapRoomBlueprintCell.blueprintCell = roomBlueprintCell;
                newMapRoomBlueprintCell.Initialize();

                gameWorldBlueprintCells.Add(newMapRoomBlueprintCell);
            }
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
            ResetBlueprintCells();
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            ResetBlueprintCells();
        }
    }
}
