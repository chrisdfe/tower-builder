using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Map;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
{
    public class GameWorldRoomCell : MonoBehaviour
    {
        static Color HOVER_COLOR = new Color(1, 0, 0, 0.4f);
        public RoomCell roomCell;

        public Color baseColor;

        public List<GameWorldRoomEntrance> gameWorldRoomEntrances = new List<GameWorldRoomEntrance>();

        GameObject gameWorldRoomEntrancePrefab;

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
            InitializeRoomCellMeshSegments();
            InitializeRoomEntrances();
            ResetColor();
        }

        public void ResetEntrances()
        {
            foreach (GameWorldRoomEntrance gameWorldRoomEntrance in gameWorldRoomEntrances)
            {
                gameWorldRoomEntrance.ResetColor();
            }
        }

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

        void Awake()
        {
            transform.localPosition = Vector3.zero;

            gameWorldRoomEntrancePrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");

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
            // Registry.Stores.MapUI.buildToolSubState.onBlueprintRoomConnectionsUpdated -= OnBlueprintRoomConnectionsUpdated;
        }

        void InitializeRoomCellMeshSegments()
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

            void SetEnabled(Transform segment, bool enabled)
            {
                segment.GetComponent<MeshRenderer>().enabled = enabled;
            }
        }

        void InitializeRoomEntrances()
        {
            foreach (RoomEntrance roomEntrance in roomCell.entrances)
            {
                GameObject roomEntranceGameObject = Instantiate<GameObject>(gameWorldRoomEntrancePrefab);
                roomEntranceGameObject.transform.parent = transform;

                GameWorldRoomEntrance gameWorldRoomEntrance = roomEntranceGameObject.GetComponent<GameWorldRoomEntrance>();
                gameWorldRoomEntrance.roomEntrance = roomEntrance;
                // gameWorldRoomEntrance.parentGameWorldRoomCell = this;
                gameWorldRoomEntrance.Initialize();
                gameWorldRoomEntrances.Add(gameWorldRoomEntrance);
            }
        }

        void UpdateRoomEntrances()
        {
            foreach (GameWorldRoomEntrance gameWorldRoomEntrance in gameWorldRoomEntrances)
            {
                gameWorldRoomEntrance.ResetColor();
            }

        }

        void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomCell.coordinates);
        }
    }
}