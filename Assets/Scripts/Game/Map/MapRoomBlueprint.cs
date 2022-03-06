using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class MapRoomBlueprint : MonoBehaviour
    {
        public bool isEnabled { get; private set; } = true;

        public RoomBlueprint roomBlueprint { get; private set; }

        GameObject mapRoomBlueprintCellPrefab;

        List<MapRoomBlueprintCell> mapRoomBlueprintCells;

        public void ResetCursorCells()
        {
            DestroyBlueprintCells();

            RoomBlueprint blueprint = Registry.Stores.MapUI.currentBlueprint;

            foreach (RoomBlueprintCell roomBlueprintCell in blueprint.roomBlueprintCells)
            {
                GameObject newMapRoomBlueprintCellGameObject = Instantiate<GameObject>(mapRoomBlueprintCellPrefab);

                MapRoomBlueprintCell newMapRoomBlueprintCell = newMapRoomBlueprintCellGameObject.GetComponent<MapRoomBlueprintCell>();
                // newMapRoomBlueprintCell.parentRoomBlueprint = this;
                newMapRoomBlueprintCell.transform.SetParent(transform);
                newMapRoomBlueprintCell.SetRoomBlueprintCell(roomBlueprintCell);
                newMapRoomBlueprintCell.UpdateMaterialColor();

                mapRoomBlueprintCells.Add(newMapRoomBlueprintCell);
            }
        }

        void Awake()
        {
            mapRoomBlueprintCellPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapRoomBlueprintCell");

            mapRoomBlueprintCells = new List<MapRoomBlueprintCell>();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnDestroy()
        {
            DestroyBlueprintCells();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.onSelectedRoomKeyUpdated -= OnSelectedRoomKeyUpdated;
        }

        void DestroyBlueprintCells()
        {
            if (mapRoomBlueprintCells.Count > 0)
            {
                foreach (MapRoomBlueprintCell mapRoomBlueprintCell in mapRoomBlueprintCells)
                {
                    Destroy(mapRoomBlueprintCell.gameObject);
                }
            }

            mapRoomBlueprintCells = new List<MapRoomBlueprintCell>();
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