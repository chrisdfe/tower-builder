using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    public class RoomCellEntityMeshWrapper : EntityMeshCellWrapper
    {
        protected Dictionary<string, Transform> segments = new Dictionary<string, Transform>();

        public RoomCellEntityMeshWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) : base(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition) { }

        public override void Setup()
        {
            base.Setup();

            SetSegments();
            UpdateMesh();
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

    class RoomCellDefaultMeshWrapper : RoomCellEntityMeshWrapper
    {
        public RoomCellDefaultMeshWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) : base(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition) { }

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
            Debug.Log("Setting segments");
            Debug.Log(meshTransform);
            segments = new Dictionary<string, Transform>() {
                    { "leftWall",           meshTransform.Find("Wrapper").Find("LeftWall").Find("LeftWallFull") },
                    { "rightWall",          meshTransform.Find("Wrapper").Find("RightWall").Find("RightWallFull") },
                    { "backWallFull",       meshTransform.Find("Wrapper").Find("BackWall").Find("BackWallFull") },
                    { "backWallWithWindow", meshTransform.Find("Wrapper").Find("BackWall").Find("BackWall__Window") },
                    { "ceiling",            meshTransform.Find("Wrapper").Find("Ceiling").Find("CeilingFull") },
                    { "floor",              meshTransform.Find("Wrapper").Find("Floor").Find("FloorFull") },
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

            string[] segmentsToEnable = cellPosition switch
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

    class RoomCellWheelsMeshWrapper : RoomCellEntityMeshWrapper
    {
        public RoomCellWheelsMeshWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) : base(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition) { }

        public override void SetSegments()
        {
            segments = new Dictionary<string, Transform>() {
                { "tire", meshTransform.Find("Tire") },
                { "hubcap", meshTransform.Find("Hubcap") },
            };
        }

        public override void SetColor(Color color)
        {
            SetSegmentColor(segments["tire"], color);
        }
    }
}