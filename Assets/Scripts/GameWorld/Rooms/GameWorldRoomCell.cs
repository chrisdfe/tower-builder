using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
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

        [HideInInspector]
        public Room room;

        [HideInInspector]
        public CellCoordinates cellCoordinates;

        [HideInInspector]
        public OccupiedCellMap occupiedCellMap;

        [HideInInspector]
        public Tileable tileable;

        [HideInInspector]
        public Tileable.CellPosition cellPosition;

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
            // TODO - not this hardcoded string
            TransformUtils.DestroyChildren(transform.Find("RoomCellMesh_Default"));
            AssetList<GameWorldRoomsManager.MeshAssetKey> assetList = GameWorldRoomsManager.Find().meshAssets;

            roomCellMeshWrapper = gameWorldRoom.room.skin.key switch
            {
                Room.Skin.Key.Wheels => new RoomCellWheelsMeshWrapper(transform, assetList, this),
                Room.Skin.Key.Default => new RoomCellDefaultMeshWrapper(transform, assetList, this),
                _ => new RoomCellDefaultMeshWrapper(transform, assetList, this)
            };

            roomCellMeshWrapper.Setup();

            UpdateLights();
            UpdateMesh();
            UpdatePosition();
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
            Transform lightTransform = transform.Find("Light");

            if (room.skin.config.hasInteriorLights)
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
            transform.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates);
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
            GameObject prefab = roomsManager.prefabAssets.FindByKey(GameWorldRoomsManager.AssetKey.RoomCell);
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

                string[] segmentsToEnable = gameWorldRoomCell.cellPosition switch
                {
                    Tileable.CellPosition.Top => new[] { "ceiling" },
                    Tileable.CellPosition.TopRight => new[] { "ceiling", "rightWall" },
                    Tileable.CellPosition.Right => new[] { "rightWall" },
                    Tileable.CellPosition.BottomRight => new[] { "rightWall", "floor" },
                    Tileable.CellPosition.Bottom => new[] { "floor" },
                    Tileable.CellPosition.BottomLeft => new[] { "floor", "leftWall" },
                    Tileable.CellPosition.Left => new[] { "leftWall" },
                    Tileable.CellPosition.TopLeft => new[] { "leftWall", "ceiling" },
                    Tileable.CellPosition.TopIsolated => new[] { "leftWall", "ceiling", "rightWall", },
                    Tileable.CellPosition.RightIsolated => new[] { "ceiling", "rightWall", "floor" },
                    Tileable.CellPosition.BottomIsolated => new[] { "leftWall", "rightWall", "floor" },
                    Tileable.CellPosition.LeftIsolated => new[] { "leftWall", "ceiling", "floor" },
                    Tileable.CellPosition.Single => new[] { "leftWall", "ceiling", "rightWall", "floor" },
                    Tileable.CellPosition.Center => new string[0],
                    Tileable.CellPosition.HorizontalCenter => new[] { "ceiling", "floor" },
                    Tileable.CellPosition.VerticalCenter => new[] { "leftWall", "rightWall" },
                    _ => new string[0],
                };

                foreach (string key in segmentsToEnable)
                {
                    SetSegmentEnabled(segments[key], true);
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