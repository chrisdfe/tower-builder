using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class BuildingWrapper : MonoBehaviour
    {
        GameObject mapCubeCellPrefab;

        // id: list of cells that belong to the room
        List<MapRoomCell> mapRoomCells = new List<MapRoomCell>();

        void Awake()
        {
            mapCubeCellPrefab = Resources.Load<GameObject>("Prefabs/Map/MapRoomCell");


            // TODO - populate rooms based on initial state of map
            //        instead of just an empty list
            mapRoomCells = new List<MapRoomCell>();

            Registry.Stores.Map.onRoomAdded += OnRoomAdded;
            Registry.Stores.Map.onRoomDestroyed += OnRoomDestroyed;
        }

        void OnRoomAdded(Room newMapRoom)
        {
            CreateRoom(newMapRoom);
        }

        void CreateRoom(Room room)
        {
            foreach (CellCoordinates cellCoordinates in room.roomCells.cells)
            {
                GameObject mapRoomCellGameObject = Instantiate<GameObject>(mapCubeCellPrefab);
                MapRoomCell mapRoomCell = mapRoomCellGameObject.GetComponent<MapRoomCell>();

                mapRoomCell.transform.SetParent(transform);
                mapRoomCell.SetMapRoom(room);
                mapRoomCell.SetRoomCell(cellCoordinates);
                mapRoomCell.Initialize();

                mapRoomCells.Add(mapRoomCell);
            }
        }

        void OnRoomDestroyed(Room room)
        {
            if (room == null)
            {
                return;
            }

            List<MapRoomCell> newMapRoomCells = new List<MapRoomCell>();

            foreach (MapRoomCell mapRoomCell in mapRoomCells)
            {
                if (mapRoomCell.room.id == room.id)
                {
                    Destroy(mapRoomCell.gameObject);
                }
                else
                {
                    newMapRoomCells.Add(mapRoomCell);
                }
            }

            mapRoomCells = newMapRoomCells;
        }
    }
}
