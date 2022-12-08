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

            // middle walls
            Top,
            Right,
            Bottom,
            Left,

            // Centers
            Center,
            HorizontalCenter,
            VerticalCenter,
            LowToHighDiagonalCenter,
            HighToLowDiagonalCenter,

            // corners
            TopLeft,
            TopRight,
            BottomRight,
            BottomLeft,

            // Isolated cells
            TopIsolated,
            RightIsolated,
            BottomIsolated,
            LeftIsolated,

            TopRightIsolated,
            BottomRightIsolated,
            BottomLeftIsolated,
            TopLeftIsolated,
        }

        // Map of CellPosition enum -> fbx node names
        public static EnumStringMap<Tileable.CellPosition> CellPositionLabelMap = new EnumStringMap<Tileable.CellPosition>(
            new Dictionary<CellPosition, string>() {
                { CellPosition.None,                   "None" },
                { CellPosition.Single,                 "Single" },

                { CellPosition.Top,                    "Top" },
                { CellPosition.Right,                  "Right" },
                { CellPosition.Bottom,                 "Bottom" },
                { CellPosition.Left,                   "Left" },

                { CellPosition.Center,                  "Center" },
                { CellPosition.HorizontalCenter,        "HorizontalCenter" },
                { CellPosition.VerticalCenter,          "VerticalCenter" },
                { CellPosition.LowToHighDiagonalCenter, "LowToHighDiagonalCenter" },
                { CellPosition.HighToLowDiagonalCenter, "HighToLowDiagonalCenter" },

                { CellPosition.TopLeft,                 "TopLeft" },
                { CellPosition.TopRight,                "TopRight" },
                { CellPosition.BottomLeft,              "BottomLeft" },
                { CellPosition.BottomRight,             "BottomRight" },

                { CellPosition.TopIsolated,             "TopIsolated" },
                { CellPosition.BottomIsolated,          "BottomIsolated" },
                { CellPosition.LeftIsolated,            "LeftIsolated" },
                { CellPosition.RightIsolated,           "RightIsolated" },

                { CellPosition.TopRightIsolated,        "TopRightIsolated" },
                { CellPosition.BottomRightIsolated,     "BottomRightIsolated" },
                { CellPosition.BottomLeftIsolated,      "BottomLeftIsolated" },
                { CellPosition.TopLeftIsolated,         "TopLeftIsolated" },
            }
        );

        public static CellPosition GetCellPosition(CellNeighbors cellNeighbors)
        {
            CellPosition cellPosition = GetOrthagonalCellPosition(cellNeighbors);

            if (cellPosition == CellPosition.Single)
            {
                CellPosition diagonalCellPosition = GetDiagonalCellPositions(cellNeighbors);

                if (diagonalCellPosition != CellPosition.Single)
                {
                    cellPosition = diagonalCellPosition;
                }
            }

            return cellPosition;
        }

        static CellPosition GetOrthagonalCellPosition(CellNeighbors cellNeighbors) =>
            cellNeighbors.occupied switch
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
                (CellOrientation.BelowLeft | CellOrientation.AboveRight) => Tileable.CellPosition.LowToHighDiagonalCenter,
                (CellOrientation.AboveLeft | CellOrientation.BelowRight) => Tileable.CellPosition.HighToLowDiagonalCenter,

                // 1 side
                (CellOrientation.Above) => Tileable.CellPosition.BottomIsolated,
                (CellOrientation.AboveRight) => Tileable.CellPosition.BottomLeftIsolated,

                (CellOrientation.Right) => Tileable.CellPosition.LeftIsolated,
                (CellOrientation.BelowRight) => Tileable.CellPosition.TopLeftIsolated,

                (CellOrientation.Below) => Tileable.CellPosition.TopIsolated,
                (CellOrientation.BelowLeft) => Tileable.CellPosition.TopRightIsolated,

                (CellOrientation.Left) => Tileable.CellPosition.RightIsolated,
                (CellOrientation.AboveLeft) => Tileable.CellPosition.BottomRightIsolated,

                // Default
                _ => Tileable.CellPosition.Single
            };

        static CellPosition GetDiagonalCellPositions(CellNeighbors cellNeighbors) =>
            cellNeighbors.occupied switch
            {
                // centers
                (CellOrientation.BelowLeft | CellOrientation.AboveRight) => Tileable.CellPosition.LowToHighDiagonalCenter,
                (CellOrientation.AboveLeft | CellOrientation.BelowRight) => Tileable.CellPosition.HighToLowDiagonalCenter,

                // 1 side
                (CellOrientation.AboveRight) => Tileable.CellPosition.BottomLeftIsolated,
                (CellOrientation.BelowRight) => Tileable.CellPosition.TopLeftIsolated,
                (CellOrientation.BelowLeft) => Tileable.CellPosition.TopRightIsolated,
                (CellOrientation.AboveLeft) => Tileable.CellPosition.BottomRightIsolated,

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