using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public abstract class Tileable
    {
        public const string TILEABLE_WRAPPER_NODE_NAME = "TileableWrapper";

        public enum Type
        {
            None,
            Single,
            Horizontal,
            Vertical,
            Diagonal,
            Full
        }

        // Map of CellPosition enum -> fbx node names
        public static EnumStringMap<Tileable.Type> TypeLabelMap = new EnumStringMap<Tileable.Type>(
            new Dictionary<Type, string>() {
                { Type.None, "None" },
                { Type.Single, "Single" },
                { Type.Horizontal, "Horizontal" },
                { Type.Vertical, "Vertical" },
                { Type.Diagonal, "Diagonal" },
                { Type.Full, "Full" },
            }
        );

        // Cell position in relation to a tileable
        public enum CellPosition
        {
            None,

            // Single (isolated)
            Single,

            // Horizontal
            Left, HorizontalCenter, Right,

            // Vertical
            Top, VerticalCenter, Bottom,

            // Diagonal
            DiagonalCenter,

            // Full
            TopLeft, TopRight,
            Center,
            BottomRight, BottomLeft,

            TopIsolated,
            BottomIsolated,
            LeftIsolated,
            RightIsolated,
        }

        // Map of CellPosition enum -> fbx node names
        public static EnumStringMap<Tileable.CellPosition> CellPositionLabelMap = new EnumStringMap<Tileable.CellPosition>(
            new Dictionary<CellPosition, string>() {
                { CellPosition.Single,           "Single" },
                { CellPosition.Left,             "Left" },
                { CellPosition.HorizontalCenter, "HorizontalCenter" },
                { CellPosition.Right,            "Right" },
                { CellPosition.Top,              "Top" },
                { CellPosition.VerticalCenter,   "VerticalCenter" },
                { CellPosition.Bottom,           "Bottom" },
                { CellPosition.TopLeft,          "TopLeft" },
                { CellPosition.TopRight,         "TopRight" },
                { CellPosition.Center,           "Center" },
                { CellPosition.BottomLeft,       "BottomLeft" },
                { CellPosition.BottomRight,      "BottomRight" },
            }
        );

        public abstract Type type { get; }
        public abstract CellPosition GetCellPosition(OccupiedCellMap occupied);

        public virtual CellPosition[] allPossibleCellPositions { get => new CellPosition[] { }; }

        public void ProcessModel(Transform model, OccupiedCellMap occupiedCellMap)
        {
            Transform tileabileWrapper = TransformUtils.FindDeepChild(model, TILEABLE_WRAPPER_NODE_NAME);

            if (tileabileWrapper == null)
            {
                Debug.Log("No TileableWrapper found");
                return;
            }

            Transform child = tileabileWrapper.GetChild(0);

            if (child == null)
            {
                return;
            }

            CellPosition cellPosition = GetCellPosition(occupiedCellMap);

            foreach (Transform node in child)
            {
                CellPosition nodeCellPosition = CellPositionLabelMap.KeyFromValue(node.name);
                node.localPosition = Vector3.zero;
                node.gameObject.SetActive(nodeCellPosition == cellPosition);
            }
        }

        /*
            Static API
        */
        public static Type TypeFromModel(Transform model)
        {
            Transform tileabileWrapper = TransformUtils.FindDeepChild(model, TILEABLE_WRAPPER_NODE_NAME);

            if (tileabileWrapper == null)
            {
                Debug.Log("No TileableWrapper found");
                return Type.None;
            }

            Transform child = tileabileWrapper.GetChild(0);

            if (child != null)
            {
                Type childNameToType = TypeLabelMap.KeyFromValue(child.name);
                return childNameToType;
            }

            return Type.None;
        }

        public static Tileable FromType(Type tileability)
        {
            return tileability switch
            {
                Type.Single => new SingleTileable(),
                Type.Horizontal => new HorizontalTileable(),
                Type.Vertical => new VerticalTileable(),
                Type.Diagonal => new DiagonalTileable(),
                Type.Full => new FullyTileable(),
                _ => null
            };
        }

        public static Tileable FromModel(Transform model)
        {
            Type type = TypeFromModel(model);
            return FromType(type);
        }

        public static Tileable FromResizability(Entity.Resizability resizability)
        {
            return resizability switch
            {
                Entity.Resizability.Flexible => new FullyTileable(),
                Entity.Resizability.Horizontal => new HorizontalTileable(),
                Entity.Resizability.Vertical => new VerticalTileable(),
                Entity.Resizability.Inflexible => new SingleTileable(),
                _ => new SingleTileable()
            };
        }

        public static List<(CellCoordinates, OccupiedCellMap)> CreateOccupiedCellMaps(CellCoordinatesList cellCoordinatesList) =>
            cellCoordinatesList.items.Select((cellCoordinates) =>
                (
                    cellCoordinates,
                    OccupiedCellMap.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList)
                )
            ).ToList();
    }
}