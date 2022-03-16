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

        Transform roomCellMesh;
        // Material roomCellMeshMaterial;

        Transform wrapperSegment;
        Transform leftWallSegment;
        Transform rightWallSegment;
        Transform ceilingSegment;
        Transform floorSegment;
        Transform backWallSegment;

        Transform[] segments;
        Transform[] wallSegments;

        public void SetRoomCell(RoomCell roomCell)
        {
            this.roomCell = roomCell;
        }

        public void Initialize()
        {
            UpdatePosition();
            SetupRoomCellMeshSegments();
            ResetColor();

            // TODO - initialize entrances
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;

            mapRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");

            roomCellMesh = transform.Find("RoomCellMesh");
            // roomCellMeshMaterial = roomCellMesh.GetComponent<Renderer>().material;

            wrapperSegment = roomCellMesh.Find("Wrapper");

            leftWallSegment = wrapperSegment.Find("LeftWall").Find("LeftWallFull");
            rightWallSegment = wrapperSegment.Find("RightWall").Find("RightWallFull");
            backWallSegment = wrapperSegment.Find("BackWall").Find("BackWallFull");
            ceilingSegment = wrapperSegment.Find("Ceiling").Find("CeilingFull");
            floorSegment = wrapperSegment.Find("Floor").Find("FloorFull");

            wallSegments = new Transform[] {
                leftWallSegment,
                rightWallSegment,
                ceilingSegment,
                floorSegment
            };

            segments = new Transform[] {
                leftWallSegment,
                rightWallSegment,
                backWallSegment,
                ceilingSegment,
                floorSegment
            };

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

            if (room.id == roomCell.roomCells.room.id)
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
            return currentInspectedRoom != null && currentInspectedRoom.id == roomCell.roomCells.room.id;
        }

        void OnInspectRoomUpdated(Room currentInspectRoom)
        {
            if (currentInspectRoom != null && currentInspectRoom.id == roomCell.roomCells.room.id)
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
            SetColor(roomCell.roomCells.room.roomDetails.color, 0.4f);
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
            foreach (Transform segment in segments)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                Color currentColor = material.color;
                material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
        }

        void SetColor(Color color, float alpha)
        {
            foreach (Transform segment in wallSegments)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                Color currentColor = material.color;
                material.color = new Color(color.r, color.g, color.b, alpha);
            }
        }

        void ResetColor()
        {
            SetColor(roomCell.roomCells.room.roomDetails.color, 1f);
        }

        void SetupRoomCellMeshSegments()
        {
            foreach (Transform segment in wallSegments)
            {
                SetEnabled(segment, false);
            }

            foreach (RoomCellPosition cellPosition in roomCell.position)
            {
                switch (cellPosition)
                {
                    case RoomCellPosition.Top:
                        SetEnabled(ceilingSegment, true);
                        break;
                    case RoomCellPosition.Right:
                        SetEnabled(rightWallSegment, true);
                        break;
                    case RoomCellPosition.Bottom:
                        SetEnabled(floorSegment, true);
                        break;
                    case RoomCellPosition.Left:
                        SetEnabled(leftWallSegment, true);
                        break;
                }
            }
        }

        void SetEnabled(Transform segment, bool enabled)
        {
            segment.GetComponent<MeshRenderer>().enabled = enabled;
        }
    }
}