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
    public Room room { get; private set; }

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

    public void SetMapRoom(Room room)
    {
        this.room = room;
    }

    public void SetRoomCell(CellCoordinates cellCoordinates)
    {
        this.cellCoordinates = cellCoordinates;
    }

    public void Setup()
    {
        transform.position = MapCellHelpers.CellCoordinatesToPosition(cellCoordinates);

        // Set color
        MapRoomDetails mapRoomDetails = TowerBuilder.Stores.Map.Constants.ROOM_DETAILS_MAP[room.roomKey];
        Color color = mapRoomDetails.color;
        cellCubeMaterial.color = mapRoomDetails.color;
    }
}
