using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.GameWorld;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomCell : MonoBehaviour
    {
        public enum ColorKey
        {
            Base,
            Hover,
            Inspected,
            Destroy,
            ValidBlueprint,
            InvalidBlueprint
        }

        public static Dictionary<ColorKey, Color> ColorMap = new Dictionary<ColorKey, Color>() {
            { ColorKey.Base, Color.grey },
            { ColorKey.Hover, Color.green },
            { ColorKey.Inspected, Color.cyan },
            { ColorKey.Destroy, Color.red },
            { ColorKey.ValidBlueprint, Color.blue },
            { ColorKey.InvalidBlueprint, Color.red },
        };

        public RoomCell roomCell;
        public GameWorldRoom gameWorldRoom;
        public Color baseColor;

        RoomCellMeshWrapperBase roomCellMeshWrapper;

        /* 
            Lifecycle methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            TransformUtils.DestroyChildren(transform);

            switch (gameWorldRoom.room.skinKey)
            {
                case RoomSkinKey.Wheels:
                    roomCellMeshWrapper = new RoomCellWheelsMeshWrapper(this);
                    break;
                case RoomSkinKey.Default:
                    roomCellMeshWrapper = new RoomCellDefaultMeshWrapper(this);
                    break;
            }
            roomCellMeshWrapper.Setup();

            UpdatePosition();
            UpdateMesh();
        }

        public void Teardown()
        {
            roomCellMeshWrapper.Teardown();
        }

        /* 
            Public Interface
        */
        public void UpdatePosition()
        {
            transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomCell.coordinates);
            // roomCellMeshWrapper.UpdatePosition();
        }

        public void UpdateMesh()
        {
            roomCellMeshWrapper.UpdateMesh();
        }

        public void SetColor(ColorKey key)
        {
            Color color;
            if (key == ColorKey.Base)
            {
                color = baseColor;
            }
            else
            {
                color = ColorMap[key];
            }
            roomCellMeshWrapper.SetColor(color);
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

        /*
            Internal classes
        */
        abstract class RoomCellMeshWrapperBase
        {
            public virtual string meshName { get; }
            protected Transform meshTransform;
            protected GameWorldRoomCell gameWorldRoomCell;

            protected Transform wrapper;

            protected Dictionary<string, Transform> segments = new Dictionary<string, Transform>();

            public RoomCellMeshWrapperBase(GameWorldRoomCell gameWorldRoomCell)
            {
                this.gameWorldRoomCell = gameWorldRoomCell;
            }

            public virtual void Setup()
            {
                LoadModel();

                wrapper = meshTransform.Find("Wrapper");

                SetSegments();
            }

            public virtual void Teardown()
            {
                GameObject.Destroy(meshTransform.gameObject);
            }

            public virtual void UpdateMesh() { }

            public abstract void SetSegments();
            public abstract void SetColor(Color color);

            protected void LoadModel()
            {
                GameObject mesh = AssetsManager.Find().FindByName(meshName);
                Transform meshTransform = Instantiate(mesh).GetComponent<Transform>();
                meshTransform.SetParent(gameWorldRoomCell.transform);
                this.meshTransform = meshTransform;
            }

            protected void SetSegmentEnabled(Transform segment, bool enabled)
            {
                segment.GetComponent<MeshRenderer>().enabled = enabled;
            }

            protected void SetSegmentColor(Transform segment, Color color)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                material.color = color;
            }
        }

        class RoomCellDefaultMeshWrapper : RoomCellMeshWrapperBase
        {
            public override string meshName { get { return "RoomCellMesh_Default"; } }

            public RoomCellDefaultMeshWrapper(GameWorldRoomCell gameWorldRoomCell) : base(gameWorldRoomCell) { }

            public override void SetColor(Color color)
            {
                foreach (Transform segment in wallSegments)
                {
                    SetSegmentColor(segment, color);
                }
            }

            Transform[] wallSegments;

            public override void SetSegments()
            {
                segments = new Dictionary<string, Transform>() {
                    { "leftWall",           wrapper.Find("LeftWall").Find("LeftWallFull") },
                    { "rightWall",          wrapper.Find("RightWall").Find("RightWallFull") },
                    { "backWallFull",       wrapper.Find("BackWall").Find("BackWallFull") },
                    { "backWallWithWindow", wrapper.Find("BackWall").Find("BackWall__Window") },
                    { "ceiling",            wrapper.Find("Ceiling").Find("CeilingFull") },
                    { "floor",              wrapper.Find("Floor").Find("FloorFull") },
                };

                wallSegments = new Transform[] {
                    segments["ceiling"],
                    segments["leftWall"],
                    segments["rightWall"],
                    segments["floor"],
                };
            }

            public override void UpdateMesh()
            {
                SetSegmentEnabled(segments["backWallFull"], false);
                SetSegmentEnabled(segments["backWallWithWindow"], true);

                foreach (Transform segment in wallSegments)
                {
                    SetSegmentEnabled(segment, false);
                }

                foreach (RoomCellOrientation cellPosition in gameWorldRoomCell.roomCell.orientation)
                {
                    switch (cellPosition)
                    {
                        case RoomCellOrientation.Top:
                            SetSegmentEnabled(segments["ceiling"], true);
                            break;
                        case RoomCellOrientation.Right:
                            SetSegmentEnabled(segments["rightWall"], true);
                            break;
                        case RoomCellOrientation.Bottom:
                            SetSegmentEnabled(segments["floor"], true);
                            break;
                        case RoomCellOrientation.Left:
                            SetSegmentEnabled(segments["leftWall"], true);
                            break;
                    }
                }
            }
        }

        class RoomCellWheelsMeshWrapper : RoomCellMeshWrapperBase
        {
            public override string meshName { get { return "RoomCellMesh_Wheel"; } }

            public RoomCellWheelsMeshWrapper(GameWorldRoomCell gameWorldRoomCell) : base(gameWorldRoomCell) { }

            public override void SetSegments()
            {
                segments = new Dictionary<string, Transform>() {
                    { "tire", wrapper.Find("Tire") },
                    { "hubcap", wrapper.Find("Hubcap") },
                };
            }

            public override void SetColor(Color color)
            {
                SetSegmentColor(segments["tire"], color);
            }
        }
    }
}