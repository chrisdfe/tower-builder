using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Map;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
{
    public class GameWorldRoomCell : MonoBehaviour
    {
        public RoomCell roomCell { get; private set; }

        GameObject mapRoomEntrancePrefab;

        Transform cellCube;
        Material cellCubeMaterial;

        void Awake()
        {
            mapRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");

            cellCube = transform.Find("CellCube");
            cellCubeMaterial = cellCube.GetComponent<Renderer>().material;

            // TODO - initialize entrances

            Registry.Stores.MapUI.destroyToolSubState.onCurrentSelectedRoomUpdated += OnDestroyRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentSelectedRoomUpdated += OnInspectHoverRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
        }

        public void SetRoomCell(RoomCell roomCell)
        {
            this.roomCell = roomCell;
        }

        public void Initialize()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomCell.coordinates);

            // Set color
            ResetColor();
        }


        void OnDestroyRoomUpdated(Room currentDestroyRoom)
        {
            if (currentDestroyRoom != null && currentDestroyRoom.id == roomCell.room.id)
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
            // If this room is already being inspected then don't do anything
            if (
                Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom != null &&
                roomCell.room.id == Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom.id
            )
            {
                return;
            }

            if (currentInspectHoverRoom != null && currentInspectHoverRoom.id == roomCell.room.id)
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
            if (currentInspectRoom != null && currentInspectRoom.id == roomCell.room.id)
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
            RoomDetails RoomDetails = roomCell.room.roomDetails;
            Color color = RoomDetails.color;
            // TODO - remove (debug)
            SetColorAlpha(0.7f);
            cellCubeMaterial.color = RoomDetails.color;
        }
    }
}