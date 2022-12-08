using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public partial class Tileable
    {
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

        public static CellPosition GetCellPosition(CellNeighbors cellNeighbors) =>
            cellNeighbors.occupiedOrthogonal switch
            {
                // 4 sides
                (CellOrientation.Above | CellOrientation.Right | CellOrientation.Below | CellOrientation.Left) =>
                    Tileable.CellPosition.Center,

                // 3 sides
                (CellOrientation.Right | CellOrientation.Below | CellOrientation.Left) => Tileable.CellPosition.Top,
                (CellOrientation.Below | CellOrientation.Left | CellOrientation.Above) => Tileable.CellPosition.Right,
                (CellOrientation.Left | CellOrientation.Above | CellOrientation.Right) => Tileable.CellPosition.Bottom,
                (CellOrientation.Above | CellOrientation.Right | CellOrientation.Below) => Tileable.CellPosition.Left,

                // 2 sides
                // corners
                (CellOrientation.Above | CellOrientation.Right) => Tileable.CellPosition.BottomLeft,
                (CellOrientation.Right | CellOrientation.Below) => Tileable.CellPosition.TopLeft,
                (CellOrientation.Below | CellOrientation.Left) => Tileable.CellPosition.TopRight,
                (CellOrientation.Left | CellOrientation.Above) => Tileable.CellPosition.BottomRight,

                // centers
                (CellOrientation.Left | CellOrientation.Right) => Tileable.CellPosition.HorizontalCenter,
                (CellOrientation.Above | CellOrientation.Below) => Tileable.CellPosition.VerticalCenter,
                // TODO - diagonals

                // 1 side
                (CellOrientation.Above) => Tileable.CellPosition.BottomIsolated,
                (CellOrientation.Right) => Tileable.CellPosition.LeftIsolated,
                (CellOrientation.Below) => Tileable.CellPosition.TopIsolated,
                (CellOrientation.Left) => Tileable.CellPosition.RightIsolated,

                // Default
                _ => Tileable.CellPosition.Single
            };

        public static List<(CellCoordinates, CellNeighbors)> CreateCellNeighborMaps(CellCoordinatesList cellCoordinatesList) =>
            cellCoordinatesList.items.Select((cellCoordinates) =>
                (
                    cellCoordinates,
                    CellNeighbors.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList)
                )
            ).ToList();
    }
}