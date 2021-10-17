using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class MapManager : MonoBehaviour
    {
        Transform floorPlane;
        Collider floorPlaneCollider;
        Transform buildingWrapper;

        GameObject placeholderTileCube;

        MapCursor mapCursor;
        GameObject mapCursorPrefab;

        Vector2 currentHoveredTile;

        void Awake()
        {
            floorPlane = transform.Find("FloorPlane");
            floorPlaneCollider = floorPlane.GetComponent<Collider>();

            mapCursorPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursor");
            mapCursor = Instantiate<GameObject>(mapCursorPrefab).GetComponent<MapCursor>();
            mapCursor.transform.SetParent(transform);
            mapCursor.transform.position = Vector3.zero;

            placeholderTileCube = Resources.Load<GameObject>("Prefabs/Map/PlaceholderTileCube");

            buildingWrapper = transform.Find("BuildingWrapper");

            MapUIStore.StateChangeSelectors.onCurrentFocusFloorUpdated += OnCurrentFocusFloorUpdated;
        }

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 100))
            {
                mapCursor.SetCurrentTile(new CellCoordinates()
                {
                    x = MapCellHelpers.RoundToNearestTile(hit.point.x),
                    z = MapCellHelpers.RoundToNearestTile(hit.point.z),
                    floor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor
                });
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }

            if (Input.GetKeyDown("]"))
            {
                FocusFloorUp();
            }

            if (Input.GetKeyDown("["))
            {
                FocusFloorDown();
            }
        }

        void OnMouseDown() { }

        void OnMouseUp()
        {
            CreatePlaceholderTileOnCurrentFloor();
        }

        void CreatePlaceholderTile(CellCoordinates cellCoordinates)
        {
            GameObject placeholder = Instantiate(placeholderTileCube);
            int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
            float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;

            placeholder.transform.position = new Vector3(
                mapCursor.transform.localPosition.x,
                (currentFocusFloor * TILE_SIZE) + (TILE_SIZE / 2),
                mapCursor.transform.localPosition.z
            );
            placeholder.transform.SetParent(buildingWrapper);

            PlaceholderTileCube placeholderCube = placeholder.GetComponent<PlaceholderTileCube>();
        }

        void CreatePlaceholderTileOnCurrentFloor()
        {
            Transform cursor = mapCursor.transform;
            int tileX = MapCellHelpers.RoundToNearestTile(cursor.position.x);
            int tileZ = MapCellHelpers.RoundToNearestTile(cursor.position.z);
            int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;

            CreatePlaceholderTile(new CellCoordinates()
            {
                x = tileX,
                z = tileZ,
                floor = currentFocusFloor
            });
        }


        void FocusFloorUp()
        {
            int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
            SetFocusFloor(currentFocusFloor + 1);
        }

        void FocusFloorDown()
        {
            int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
            SetFocusFloor(currentFocusFloor - 1);
        }

        void SetFocusFloor(int floor)
        {
            Stores.MapUI.MapUIStore.Mutations.SetCurrentFocusFloor(floor);
        }

        void OnCurrentFocusFloorUpdated(MapUIStore.StateEventPayload payload)
        {
            int currentFocusFloor = payload.state.currentFocusFloor;
            float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;

            floorPlane.position = new Vector3(
                floorPlane.position.x,
                (currentFocusFloor * TILE_SIZE) + 0.01f,
                floorPlane.position.z
            );
        }

        void OnRoomAdded(MapUIStore.StateEventPayload payload) { }
    }
}
