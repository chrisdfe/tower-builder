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
    public class MapTileManager : MonoBehaviour
    {
        // Tile width/height
        public static int MAP_GRID_WIDTH = 10;
        public static int MAP_GRID_HEIGHT = 10;
        public static float TILE_SIZE = 0.5f;

        public static Color TILE_DEFAULT_COLOR = new Color(0, 0, 0, 0);
        public static Color TILE_HOVER_COLOR = new Color(1, 1, 1, 0.3f);

        GameObject mapTilePrefab;
        Transform mapTileContainer;

        MapTileManager mapTileManager;

        MapTile currentHoveredMapTile;

        public void InstantiateMapTiles()
        {
            for (int x = 0; x < MAP_GRID_WIDTH; x++)
            {
                for (int y = 0; y < MAP_GRID_HEIGHT; y++)
                {
                    InstantiateMapTile(x, y);
                }
            }
        }

        void Awake()
        {
            mapTilePrefab = Resources.Load<GameObject>("Prefabs/Map/MapTile");
        }

        private void InstantiateMapTile(int x, int y)
        {
            GameObject newMapTileGameObject = Instantiate<GameObject>(mapTilePrefab, Vector3.zero, Quaternion.identity);
            MapTile newMapTile = newMapTileGameObject.GetComponent<MapTile>();

            newMapTile.x = x;
            newMapTile.y = y;

            newMapTile.onMouseDown += OnMapTileMouseDown;
            newMapTile.onMouseUp += OnMapTileMouseUp;
            newMapTile.onMouseOver += OnMapTileMouseOver;
            newMapTile.onMouseExit += OnMapTileMouseExit;

            newMapTile.transform.name = $"MapTile ({x}, {y})";
            newMapTile.transform.localScale = new Vector3(TILE_SIZE, TILE_SIZE, 1);
            float mapTileX = (x * TILE_SIZE) - ((MAP_GRID_WIDTH * TILE_SIZE) / 2);
            float mapTileY = (y * TILE_SIZE) - ((MAP_GRID_HEIGHT * TILE_SIZE) / 2);
            newMapTile.transform.position = new Vector3(mapTileX, mapTileY, 1);

            newMapTile.transform.SetParent(transform);

            newMapTile.GetComponent<Renderer>().material.SetColor("_Color", TILE_DEFAULT_COLOR);

            // TODO - add it to local List/Dict
        }

        private void OnMapTileMouseDown(MapTile mapTile)
        {
            Debug.Log($"mapTile mousedown: {mapTile.x}, {mapTile.y}");
        }

        private void OnMapTileMouseUp(MapTile mapTile)
        {
            Debug.Log($"mapTile mouseup: {mapTile.x}, {mapTile.y}");
        }

        private void OnMapTileMouseOver(MapTile mapTile)
        {
            // Debug.Log($"mapTile mouseover: {mapTile.x}, {mapTile.y}");
            mapTile.GetComponent<Renderer>().material.SetColor("_Color", TILE_HOVER_COLOR);
        }

        private void OnMapTileMouseExit(MapTile mapTile)
        {
            mapTile.GetComponent<Renderer>().material.SetColor("_Color", TILE_DEFAULT_COLOR);
        }
    }
}
