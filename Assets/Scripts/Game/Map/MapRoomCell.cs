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

        Registry.Stores.MapUI.destroyToolSubState.onCurrentSelectedRoomUpdated += OnDestroyRoomUpdated;
    }

    public void SetMapRoom(Room room)
    {
        this.room = room;
    }

    public void SetRoomCell(CellCoordinates cellCoordinates)
    {
        this.cellCoordinates = cellCoordinates;
    }

    public void Initialize()
    {
        transform.position = MapCellHelpers.CellCoordinatesToPosition(cellCoordinates);

        // Set color
        MapRoomDetails mapRoomDetails = TowerBuilder.Stores.Map.Constants.ROOM_DETAILS_MAP[room.roomKey];
        Color color = mapRoomDetails.color;
        cellCubeMaterial.color = mapRoomDetails.color;
    }


    void OnDestroyRoomUpdated(Room currentDestroyRoom)
    {
        if (currentDestroyRoom != null && currentDestroyRoom.id == room.id)
        {
            // highlight
            setColorAlpha(0.5f);
        }
        else
        {
            setColorAlpha(1f);
        }
    }

    void setColorAlpha(float alpha)
    {
        cellCubeMaterial.color = new Color(cellCubeMaterial.color.r, cellCubeMaterial.color.g, cellCubeMaterial.color.b, alpha);
    }
}
