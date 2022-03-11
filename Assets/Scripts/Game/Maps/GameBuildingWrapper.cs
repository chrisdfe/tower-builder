using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class GameBuildingWrapper : MonoBehaviour
    {
        GameObject mapCubeCellPrefab;

        // id: list of cells that belong to the room
        List<GameRoomCell> mapRoomCells = new List<GameRoomCell>();

        void Awake()
        {
            mapCubeCellPrefab = Resources.Load<GameObject>("Prefabs/Map/GameRoomCell");

            // TODO - populate rooms based on initial state of map
            //        instead of just an empty list
            mapRoomCells = new List<GameRoomCell>();

            Registry.Stores.Map.onRoomAdded += OnRoomAdded;
            Registry.Stores.Map.onRoomDestroyed += OnRoomDestroyed;
        }

        void OnRoomAdded(Room newMapRoom)
        {
            CreateRoom(newMapRoom);
        }

        void CreateRoom(Room room)
        {
            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                GameObject mapRoomCellGameObject = Instantiate<GameObject>(mapCubeCellPrefab);
                GameRoomCell mapRoomCell = mapRoomCellGameObject.GetComponent<GameRoomCell>();

                mapRoomCell.transform.SetParent(transform);
                mapRoomCell.SetMapRoom(room);
                mapRoomCell.SetRoomCell(roomCell.coordinates);
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

            List<GameRoomCell> newGameRoomCells = new List<GameRoomCell>();

            foreach (GameRoomCell mapRoomCell in mapRoomCells)
            {
                if (mapRoomCell.room.id == room.id)
                {
                    Destroy(mapRoomCell.gameObject);
                }
                else
                {
                    newGameRoomCells.Add(mapRoomCell);
                }
            }

            mapRoomCells = newGameRoomCells;
        }
    }
}
