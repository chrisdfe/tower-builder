using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
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
            Registry.Stores.MapUI.inspectToolSubState.onCurrentSelectedRoomUpdated += OnInspectHoverRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
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
            ResetColor();
        }


        void OnDestroyRoomUpdated(Room currentDestroyRoom)
        {
            if (currentDestroyRoom != null && currentDestroyRoom.id == room.id)
            {
                // highlight
                SetColorAlpha(0.5f);
            }
            else
            {
                SetColorAlpha(1f);
            }
        }

        void OnInspectHoverRoomUpdated(Room currentInspectHoverRoom)
        {
            if (
                Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom != null &&
                room.id == Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom.id
            )
            {
                return;
            }

            if (currentInspectHoverRoom != null && currentInspectHoverRoom.id == room.id)
            {
                // highlight
                SetColorAlpha(0.5f);
            }
            else
            {
                ResetColor();
            }
        }

        void OnInspectRoomUpdated(Room currentInspectRoom)
        {
            if (currentInspectRoom != null && currentInspectRoom.id == room.id)
            {
                cellCubeMaterial.color = Color.white;
            }
            else
            {
                ResetColor();
            }
        }

        void SetColorAlpha(float alpha)
        {
            cellCubeMaterial.color = new Color(cellCubeMaterial.color.r, cellCubeMaterial.color.g, cellCubeMaterial.color.b, alpha);
        }

        void ResetColor()
        {
            RoomDetails RoomDetails = Room.GetDetails(room.roomKey);
            Color color = RoomDetails.color;
            cellCubeMaterial.color = RoomDetails.color;
        }
    }
}