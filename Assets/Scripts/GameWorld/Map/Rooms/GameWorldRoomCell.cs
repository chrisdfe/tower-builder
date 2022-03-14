using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Map;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
{
    public class GameWorldRoomCell : MonoBehaviour
    {
        public RoomCell roomCell { get; private set; }

        GameObject mapRoomEntrancePrefab;

        Transform cellCube;
        Material cellCubeMaterial;

        public void SetRoomCell(RoomCell roomCell)
        {
            this.roomCell = roomCell;
        }

        public void Initialize()
        {
            UpdatePosition();
            ResetColor();

            // TODO - initialize entrances
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;

            mapRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");

            cellCube = transform.Find("CellCube");
            cellCubeMaterial = cellCube.GetComponent<Renderer>().material;

            Registry.Stores.MapUI.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnInspectRoomUpdated;
        }

        void OnDestroy()
        {
            Registry.Stores.MapUI.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated -= OnInspectRoomUpdated;
        }

        void OnCurrentSelectedRoomUpdated(Room room)
        {
            if (room == null)
            {
                ResetColor();
                return;
            }

            if (room.id == roomCell.room.id)
            {
                switch (Registry.Stores.MapUI.toolState)
                {
                    case ToolState.Destroy:
                        SetDestroyHoverColor();
                        break;
                    default:
                        if (!IsInCurrentInspectedRoom())
                        {
                            SetHoverColor();
                        }
                        break;
                }
            }
            else
            {
                ResetColor();
            }
        }

        bool IsInCurrentInspectedRoom()
        {
            Room currentInspectedRoom = Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom;
            return currentInspectedRoom != null && currentInspectedRoom.id == roomCell.room.id;
        }

        void OnInspectRoomUpdated(Room currentInspectRoom)
        {
            if (currentInspectRoom != null && currentInspectRoom.id == roomCell.room.id)
            {
                SetInspectColor();
            }
            else
            {
                ResetColor();
            }
        }

        void SetHoverColor()
        {
            SetColor(roomCell.room.roomDetails.color, 0.4f);
        }

        void SetDestroyHoverColor()
        {
            SetColor(Color.red, 0.7f);
        }

        void SetInspectColor()
        {
            SetColor(Color.white, 0.7f);
        }

        void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomCell.coordinates);
        }

        void SetColorAlpha(float alpha)
        {
            cellCubeMaterial.color = new Color(cellCubeMaterial.color.r, cellCubeMaterial.color.g, cellCubeMaterial.color.b, alpha);
        }

        void SetColor(Color color, float alpha)
        {
            cellCubeMaterial.color = color;
            SetColorAlpha(alpha);
        }

        void ResetColor()
        {
            SetColor(roomCell.room.roomDetails.color, 1f);
        }
    }
}