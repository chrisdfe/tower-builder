using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;

public class BuildingWrapper : MonoBehaviour
{
    GameObject mapCubeCellPrefab;

    // id: list of cells that belong to the room
    Dictionary<string, List<MapRoomCell>> mapRoomCellMap = new Dictionary<string, List<MapRoomCell>>();

    void Awake()
    {
        mapCubeCellPrefab = Resources.Load<GameObject>("Prefabs/Map/MapRoomCell");

        // TODO - populate rooms based on initial state of map

        Registry.Stores.Map.onRoomAdded += OnRoomAdded;
    }

    void OnRoomAdded(Room newMapRoom)
    {
        CreateRoom(newMapRoom);
    }

    void CreateRoom(Room room)
    {
        List<MapRoomCell> mapRoomCells = new List<MapRoomCell>();

        foreach (CellCoordinates cellCoordinates in room.roomCells)
        {
            GameObject mapRoomCellGameObject = Instantiate<GameObject>(mapCubeCellPrefab);
            MapRoomCell mapRoomCell = mapRoomCellGameObject.GetComponent<MapRoomCell>();

            mapRoomCell.transform.SetParent(transform);
            mapRoomCell.SetMapRoom(room);
            mapRoomCell.SetRoomCell(cellCoordinates);
            mapRoomCell.Setup();

            mapRoomCells.Add(mapRoomCell);
        }

        mapRoomCellMap.Add(room.id, mapRoomCells);
    }
}
