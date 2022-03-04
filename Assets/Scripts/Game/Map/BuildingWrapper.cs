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

        Registry.Stores.Map.onRoomAdded += OnRoomAdded;
    }

    void OnRoomAdded(MapRoom newMapRoom)
    {
        CreateRoom(newMapRoom);
    }

    void CreateRoom(MapRoom mapRoom)
    {
        float TILE_SIZE = TowerBuilder.Stores.Map.Constants.TILE_SIZE;

        List<MapRoomCell> mapRoomCells = new List<MapRoomCell>();

        foreach (CellCoordinates cellCoordinates in mapRoom.roomCells.cells)
        {
            GameObject mapRoomCellGameObject = Instantiate<GameObject>(mapCubeCellPrefab);
            MapRoomCell mapRoomCell = mapRoomCellGameObject.GetComponent<MapRoomCell>();

            mapRoomCell.transform.SetParent(transform);
            mapRoomCell.SetMapRoom(mapRoom);
            mapRoomCell.SetRoomCell(cellCoordinates);
            mapRoomCell.Setup();

            mapRoomCells.Add(mapRoomCell);
        }

        mapRoomCellMap.Add(mapRoom.id, mapRoomCells);
    }
}
