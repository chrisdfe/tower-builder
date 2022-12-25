using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    public class RoomCellEntityMeshWrapper : EntityMeshCellWrapper
    {
        Dictionary<string, Transform> segments = new Dictionary<string, Transform>();
        Transform[] wallSegments;

        public RoomCellEntityMeshWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellCoordinates relativeCellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) : base(parent, prefabMesh, cellCoordinates, relativeCellCoordinates, cellNeighbors, cellPosition) { }

        public override void Setup()
        {
            base.Setup();

            SetSegments();
            UpdateMesh();
        }

        public void UpdateMesh()
        {
            // SetSegmentEnabled(segments["backWallFull"], false);
            // SetSegmentEnabled(segments["backWallWithWindow"], true);
            SetSegmentEnabled(segments["backWallFull"], true);
            SetSegmentEnabled(segments["backWallWithWindow"], false);

            foreach (Transform segment in wallSegments)
            {
                SetSegmentEnabled(segment, false);
            }

            string[] segmentsToEnable = cellPosition switch
            {
                Tileable.CellPosition.Top => new[] { "ceiling" },
                Tileable.CellPosition.TopRight => new[] { "ceiling", "rightWall" },
                Tileable.CellPosition.Right => new[] { "rightWall" },
                Tileable.CellPosition.BottomRight => new[] { "rightWall" },
                Tileable.CellPosition.Bottom => new string[0],
                Tileable.CellPosition.BottomLeft => new[] { "leftWall" },
                Tileable.CellPosition.Left => new[] { "leftWall" },
                Tileable.CellPosition.TopLeft => new[] { "leftWall", "ceiling" },
                Tileable.CellPosition.TopIsolated => new[] { "leftWall", "ceiling", "rightWall", },
                Tileable.CellPosition.RightIsolated => new[] { "ceiling", "rightWall" },
                Tileable.CellPosition.BottomIsolated => new[] { "leftWall", "rightWall" },
                Tileable.CellPosition.LeftIsolated => new[] { "leftWall", "ceiling" },
                Tileable.CellPosition.Single => new[] { "leftWall", "ceiling", "rightWall" },
                Tileable.CellPosition.Center => new string[0],
                Tileable.CellPosition.HorizontalCenter => new[] { "ceiling" },
                Tileable.CellPosition.VerticalCenter => new[] { "leftWall", "rightWall" },
                _ => new string[0],
            };

            foreach (string key in segmentsToEnable)
            {
                SetSegmentEnabled(segments[key], true);
            }
        }

        public void SetColor(Color color)
        {
            foreach (Transform segment in wallSegments)
            {
                SetSegmentColor(segment, color);
            }
        }

        /*
            Internals
        */
        void SetSegmentEnabled(Transform segment, bool enabled)
        {
            segment.gameObject.SetActive(enabled);
        }

        void SetSegmentColor(Transform segment, Color color)
        {
            Material material = segment.GetComponent<MeshRenderer>().material;
            material.color = color;
        }

        void SetSegments()
        {
            segments = new Dictionary<string, Transform>() {
                { "leftWall",           meshTransform.Find("Wrapper").Find("LeftWall").Find("LeftWallFull") },
                { "rightWall",          meshTransform.Find("Wrapper").Find("RightWall").Find("RightWallFull") },
                { "backWallFull",       meshTransform.Find("Wrapper").Find("BackWall").Find("BackWallFull") },
                { "backWallWithWindow", meshTransform.Find("Wrapper").Find("BackWall").Find("BackWall__Window") },
                { "ceiling",            meshTransform.Find("Wrapper").Find("Ceiling").Find("CeilingFull") },
            };

            wallSegments = new Transform[] {
                segments["ceiling"],
                segments["leftWall"],
                segments["rightWall"],
            };
        }
    }
}