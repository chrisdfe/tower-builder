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
        public bool isEnabled { get; private set; } = true;

        public Blueprint blueprint { get; private set; }

        GameObject mapRoomBlueprintCellPrefab;

        List<GameWorldBlueprintCell> mapRoomBlueprintCells;

        public void ResetCursorCells()
        {
            DestroyBlueprintCells();

            Blueprint blueprint = Registry.Stores.MapUI.buildToolSubState.currentBlueprint;

            foreach (BlueprintCell roomBlueprintCell in blueprint.roomBlueprintCells)
            {
                GameObject newMapRoomBlueprintCellGameObject = Instantiate<GameObject>(mapRoomBlueprintCellPrefab);

                GameWorldBlueprintCell newMapRoomBlueprintCell = newMapRoomBlueprintCellGameObject.GetComponent<GameWorldBlueprintCell>();

                newMapRoomBlueprintCell.transform.SetParent(transform);
                newMapRoomBlueprintCell.SetParentBlueprint(this);
                newMapRoomBlueprintCell.SetRoomBlueprintCell(roomBlueprintCell);
                newMapRoomBlueprintCell.UpdateMaterialColor();
                newMapRoomBlueprintCell.Initialize();

                mapRoomBlueprintCells.Add(newMapRoomBlueprintCell);
            }
        }

        void Awake()
        {
            mapRoomBlueprintCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Blueprints/BlueprintCell");

            mapRoomBlueprintCells = new List<GameWorldBlueprintCell>();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnDestroy()
        {
            DestroyBlueprintCells();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated -= OnSelectedRoomKeyUpdated;
        }

        void DestroyBlueprintCells()
        {
            if (mapRoomBlueprintCells.Count > 0)
            {
                foreach (GameWorldBlueprintCell mapRoomBlueprintCell in mapRoomBlueprintCells)
                {
                    Destroy(mapRoomBlueprintCell.gameObject);
                }
            }

            mapRoomBlueprintCells = new List<GameWorldBlueprintCell>();
        }


        void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            ResetCursorCells();
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            ResetCursorCells();
        }
    }
}
