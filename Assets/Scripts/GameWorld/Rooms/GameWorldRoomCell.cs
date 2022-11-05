using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.GameWorld;
using TowerBuilder.State.UI;
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

        /* 
            Lifecycle methods
        */
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
            Debug.Log("GameWorldRoomCell OnDestroy");
            GameObject.Destroy(roomCellMesh.gameObject);
        }

        public void Initialize()
        {
            SetPosition();
            UpdateRoomCellMeshSegments();
            SetBaseColor();
        }

        /* 
            Position
        */
        public void SetPosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomCell.coordinates);
        }

        /* 
            Color
        */
        public void SetBaseColor()
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

        public void SetValidBlueprintColor()
        {
            SetColor(Color.blue, 1.0f);
        }

        public void SetInvalidBlueprintColor()
        {
            SetColor(Color.red, 1.0f);
        }

        public void SetInspectColor()
        {
            SetColor(Color.white, 0.7f);
        }

        void SetColor(Color color, float alpha = 1f)
        {
            foreach (Transform segment in wallSegments)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                Color currentColor = material.color;
                material.color = new Color(color.r, color.g, color.b, alpha);
            }
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

        /* 
            Mesh Segments
         */
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

        /*
            Static API
        */
        public static GameWorldRoomCell Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomCell");
            GameObject roomCellGameObject = Instantiate<GameObject>(prefab);

            roomCellGameObject.transform.parent = parent;

            GameWorldRoomCell gameWorldRoomCell = roomCellGameObject.GetComponent<GameWorldRoomCell>();
            return gameWorldRoomCell;
        }
    }
}