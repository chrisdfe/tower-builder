using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Connections;
using TowerBuilder.Stores.Rooms.Entrances;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomCell : MonoBehaviour
    {
        static Color HOVER_COLOR = new Color(1, 0, 0, 0.4f);
        public RoomCell roomCell;

        public GameWorldRoom gameWorldRoom;

        public Color baseColor;

        public List<GameWorldRoomEntrance> gameWorldRoomEntrances = new List<GameWorldRoomEntrance>();

        Transform roomCellMesh;

        Transform wrapperSegment;
        Transform leftWallSegment;
        Transform rightWallSegment;
        Transform ceilingSegment;
        Transform floorSegment;
        Transform backWallSegment;

        Transform[] segments;
        Transform[] wallSegments;

        public void Initialize()
        {
            UpdatePosition();
            UpdateRoomCellMeshSegments();
            ResetColor();
        }

        /* 
        public void HighlightEntrance(RoomEntrance roomEntrance)
        {
            foreach (GameWorldRoomEntrance gameWorldRoomEntrance in gameWorldRoomEntrances)
            {
                if (gameWorldRoomEntrance.roomEntrance == roomEntrance)
                {
                    gameWorldRoomEntrance.SetConnectedColor();
                }
                else
                {
                    gameWorldRoomEntrance.ResetColor();
                }
            }
        }
        */

        public void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomCell.coordinates);
        }

        public void SetColor(Color color, float alpha = 1f)
        {
            foreach (Transform segment in wallSegments)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                Color currentColor = material.color;
                material.color = new Color(color.r, color.g, color.b, alpha);
            }
        }

        public void ResetColor()
        {
            SetColor(baseColor, 1f);
        }

        public void SetHoverColor()
        {
            SetColor(baseColor, 0.4f);
        }

        public void SetDestroyHoverColor()
        {
            SetColor(Color.red, 0.7f);
        }

        public void SetInspectColor()
        {
            SetColor(Color.white, 0.7f);
        }

        public void SetColorAlpha(float alpha)
        {
            foreach (Transform segment in segments)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                Color currentColor = material.color;
                material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
        }

        public void UpdateRoomCellMeshSegments()
        {
            foreach (Transform segment in wallSegments)
            {
                SetEnabled(segment, false);
            }

            foreach (RoomCellOrientation cellPosition in roomCell.orientation)
            {
                switch (cellPosition)
                {
                    case RoomCellOrientation.Top:
                        SetEnabled(ceilingSegment, true);
                        break;
                    case RoomCellOrientation.Right:
                        SetEnabled(rightWallSegment, true);
                        break;
                    case RoomCellOrientation.Bottom:
                        SetEnabled(floorSegment, true);
                        break;
                    case RoomCellOrientation.Left:
                        SetEnabled(leftWallSegment, true);
                        break;
                }
            }

            void SetEnabled(Transform segment, bool enabled)
            {
                segment.GetComponent<MeshRenderer>().enabled = enabled;
            }
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;

            roomCellMesh = transform.Find("RoomCellMesh");

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
        }

        void OnDestroy()
        {
            // Registry.Stores.UI.buildToolSubState.onBlueprintRoomConnectionsUpdated -= OnBlueprintRoomConnectionsUpdated;
        }
    }
}