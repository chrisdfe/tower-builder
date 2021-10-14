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
        MapTileManager mapTileManager;

        void Awake()
        {
            mapTileManager = transform.Find("MapTileManager").GetComponent<MapTileManager>();
        }

        void Start()
        {
            mapTileManager.InstantiateMapTiles();
        }
    }
}
