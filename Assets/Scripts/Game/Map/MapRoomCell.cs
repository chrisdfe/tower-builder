using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.UI;
using UnityEngine;

public class MapRoomCell : MonoBehaviour
{
    public MapRoom mapRoom { get; private set; }

    public CellCoordinates cellCoordinates { get; private set; }

    GameObject mapRoomCellPrefab;
    Transform cellCube;
    Material cellCubeMaterial;

    void Awake()
    {
        mapRoomCellPrefab = Resources.Load<GameObject>("Prefabs/Map/MapRoomCell");

        cellCube = transform.Find("CellCube");
        cellCubeMaterial = cellCube.GetComponent<Renderer>().material;
    }

    public void SetMapRoom(MapRoom mapRoom)
    {
        this.mapRoom = mapRoom;
    }

    public void SetRoomCell(CellCoordinates cellCoordinates)
    {
        this.cellCoordinates = cellCoordinates;
    }

    public void Setup()
    {
        // set position
        float TILE_SIZE = TowerBuilder.Stores.Map.Constants.TILE_SIZE;

        // 
        transform.position = MapCellHelpers.CellCoordinatesToPosition(cellCoordinates);

        // Set color
        MapRoomDetails mapRoomDetails = TowerBuilder.Stores.Map.Constants.ROOM_DETAILS_MAP[mapRoom.roomKey];
        Color color = mapRoomDetails.color;
        cellCubeMaterial.color = mapRoomDetails.color;
    }
}
