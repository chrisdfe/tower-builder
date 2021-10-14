using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class MapManager : MonoBehaviour
    {
        public static float TILE_SIZE = 1f;

        MapTileManager mapTileManager;

        Transform floorPlane;
        Collider floorPlaneCollider;
        Transform cubeCursor;

        Vector2 currentHoveredTile;
        int currentFocusFloor = 0;

        void Awake()
        {
            // mapTileManager = transform.Find("MapTileManager").GetComponent<MapTileManager>();
            floorPlane = transform.Find("FloorPlane");
            floorPlaneCollider = floorPlane.GetComponent<Collider>();
            cubeCursor = transform.Find("CubeCursor");
            cubeCursor.localScale = new Vector3(TILE_SIZE, TILE_SIZE, TILE_SIZE);
        }

        void Start()
        {
            // mapTileManager.InstantiateMapTiles();
        }

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 100))
            {
                cubeCursor.localPosition = SnapToTileGrid(hit.point);
                currentHoveredTile = new Vector2(
                    cubeCursor.localPosition.x / TILE_SIZE,
                    cubeCursor.localPosition.z / TILE_SIZE
                );
            }

            if (Input.GetMouseButtonDown(0))
            {
            }

            if (Input.GetMouseButtonUp(0))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(
                    cubeCursor.localPosition.x,
                    (currentFocusFloor * TILE_SIZE) + (TILE_SIZE / 2),
                    cubeCursor.localPosition.z
                );
            }

            // move floor up
            if (Input.GetKeyDown("]"))
            {
                FocusFloorUp();
            }

            if (Input.GetKeyDown("["))
            {
                FocusFloorDown();
            }
        }

        // TODO -this definitely doesn't belong here
        float RoundToNearestTile(float number)
        {
            return (float)Math.Round(number / TILE_SIZE) * TILE_SIZE;
        }

        Vector3 SnapToTileGrid(Vector3 point)
        {
            return new Vector3(
                RoundToNearestTile(point.x),
                (currentFocusFloor * TILE_SIZE) + (TILE_SIZE / 2),
                RoundToNearestTile(point.z)
            );
        }

        void FocusFloorUp()
        {
            SetFocusFloor(currentFocusFloor + 1);
        }

        void FocusFloorDown()
        {
            SetFocusFloor(currentFocusFloor - 1);
        }

        void SetFocusFloor(int floor)
        {
            currentFocusFloor = floor;
            Debug.Log($"current floor: {floor}");

            floorPlane.position = new Vector3(
                floorPlane.position.x,
                (currentFocusFloor * TILE_SIZE) + 0.01f,
                floorPlane.position.z
            );
        }
    }
}
