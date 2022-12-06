using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
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

        public class SkinConfig
        {
            public bool hasInteriorLights = false;
        }

        public static Dictionary<Room.SkinKey, SkinConfig> SkinConfigMap = new Dictionary<Room.SkinKey, SkinConfig>() {
            {
                Room.SkinKey.Default,
                new SkinConfig() {
                    hasInteriorLights = true,
                }
            },
            {
                Room.SkinKey.Wheels,
                new SkinConfig() {
                    hasInteriorLights = false,
                }
            }
        };

        [HideInInspector]
        public RoomCell roomCell;

        [HideInInspector]
        public GameWorldRoom gameWorldRoom;

        RoomCellMeshWrapper roomCellMeshWrapper;
        GameWorldRoomCellLight roomCellLight;

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
            // TODO - not this
            TransformUtils.DestroyChildren(transform.Find("RoomCellMesh_Default"));
            AssetList<GameWorldRoomsManager.MeshAssetKey> assetList = GameWorldRoomsManager.Find().meshAssetList;

            switch (gameWorldRoom.room.skinKey)
            {
                case Room.SkinKey.Wheels:
                    roomCellMeshWrapper = new RoomCellWheelsMeshWrapper(transform, assetList, this);
                    break;
                case Room.SkinKey.Default:
                    roomCellMeshWrapper = new RoomCellDefaultMeshWrapper(transform, assetList, this);
                    break;
            }

            roomCellMeshWrapper.Setup();

            UpdateLights();
            UpdatePosition();
            UpdateMesh();
        }

        public void Teardown()
        {
            roomCellMeshWrapper.Teardown();

            if (roomCellLight != null)
            {
                roomCellLight.Teardown();
            }
        }

        /* 
            Public Interface
        */
        public void UpdateLights()
        {
            SkinConfig config = SkinConfigMap[gameWorldRoom.room.skinKey];
            Transform lightTransform = transform.Find("Light");

            if (config.hasInteriorLights)
            {
                Light light = lightTransform.GetComponent<Light>();
                roomCellLight = new GameWorldRoomCellLight(light);
                roomCellLight.Setup();
            }
            else
            {
                Destroy(lightTransform.gameObject);
            }
        }

        public void UpdatePosition()
        {
            transform.position = GameWorldUtils.CellCoordinatesToPosition(roomCell.coordinates);
        }

        public void UpdateMesh()
        {
            roomCellMeshWrapper.UpdateMesh();
        }

        public void SetColor(ColorKey key)
        {
            Color color = ColorMap[key];

            roomCellMeshWrapper.SetColor(color);
        }

        /*
            Static API
        */
        public static GameWorldRoomCell Create(Transform parent)
        {
            GameWorldRoomsManager roomsManager = GameWorldRoomsManager.Find();
            GameObject prefab = roomsManager.assetList.FindByKey(GameWorldRoomsManager.AssetKey.RoomCell);
            GameObject roomCellGameObject = Instantiate<GameObject>(prefab);

            roomCellGameObject.transform.parent = parent;

            GameWorldRoomCell gameWorldRoomCell = roomCellGameObject.GetComponent<GameWorldRoomCell>();
            return gameWorldRoomCell;
        }

        /*
            Internal classes
        */
        class RoomCellMeshWrapper : MeshWrapper<GameWorldRoomsManager.MeshAssetKey>
        {
            protected GameWorldRoomCell gameWorldRoomCell;
            protected Transform wrapper;

            protected Dictionary<string, Transform> segments = new Dictionary<string, Transform>();

            public RoomCellMeshWrapper(
                Transform parent,
                AssetList<GameWorldRoomsManager.MeshAssetKey> assetList,
                GameWorldRoomsManager.MeshAssetKey key,
                GameWorldRoomCell gameWorldRoomCell)
                : base(parent, assetList, key)
            {
                this.gameWorldRoomCell = gameWorldRoomCell;
            }

            public override void Setup()
            {
                base.Setup();

                wrapper = meshTransform.Find("Wrapper");
                SetSegments();
            }

            public virtual void UpdateMesh() { }

            public virtual void SetSegments() { }
            public virtual void SetColor(Color color) { }

            protected void SetSegmentEnabled(Transform segment, bool enabled)
            {
                segment.gameObject.SetActive(enabled);
            }

            protected void SetSegmentColor(Transform segment, Color color)
            {
                Material material = segment.GetComponent<MeshRenderer>().material;
                material.color = color;
            }
        }

        class RoomCellDefaultMeshWrapper : RoomCellMeshWrapper
        {
            public RoomCellDefaultMeshWrapper(
                Transform parent,
                AssetList<GameWorldRoomsManager.MeshAssetKey> assetList,
                GameWorldRoomCell gameWorldRoomCell
            ) : base(
                parent,
                assetList,
                GameWorldRoomsManager.MeshAssetKey.Default,
                gameWorldRoomCell)
            { }

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

        class RoomCellWheelsMeshWrapper : RoomCellMeshWrapper
        {
            public RoomCellWheelsMeshWrapper(
                Transform parent,
                AssetList<GameWorldRoomsManager.MeshAssetKey> assetList,
                GameWorldRoomCell gameWorldRoomCell
            ) : base(
                parent,
                assetList,
                GameWorldRoomsManager.MeshAssetKey.Wheels,
                gameWorldRoomCell)
            { }

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