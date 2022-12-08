using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class FullyTileable : Tileable
    {
        public override Type type { get; } = Type.Full;

        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.Single,
                    CellPosition.Left,
                    CellPosition.HorizontalCenter,
                    CellPosition.Right,
                    CellPosition.Top,
                    CellPosition.VerticalCenter,
                    CellPosition.Bottom,
                    CellPosition.TopLeft,
                    CellPosition.TopRight,
                    CellPosition.Center,
                    CellPosition.BottomLeft,
                    CellPosition.BottomRight,
                    CellPosition.TopIsolated,
                    CellPosition.BottomIsolated,
                    CellPosition.LeftIsolated,
                    CellPosition.RightIsolated,
                };
        }

        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupiedCellMap)
        {
            // Centers
            if (
                occupiedCellMap.HasAll(new[] {
                    CellOrientation.Above,
                    CellOrientation.Right,
                    CellOrientation.Below,
                    CellOrientation.Left
                })
            )
            {
                return Tileable.CellPosition.Center;
            }

            if (occupiedCellMap.HasOnly(new[] { CellOrientation.Above, CellOrientation.Below }))
            {
                return Tileable.CellPosition.VerticalCenter;
            }

            if (occupiedCellMap.HasOnly(new[] { CellOrientation.Left, CellOrientation.Right }))
            {
                return Tileable.CellPosition.HorizontalCenter;
            }


            // Isolated parts
            if (occupiedCellMap.HasOnly(CellOrientation.Above))
            {
                return Tileable.CellPosition.BottomIsolated;
            }

            if (occupiedCellMap.HasOnly(CellOrientation.Right))
            {
                return Tileable.CellPosition.LeftIsolated;
            }

            if (occupiedCellMap.HasOnly(CellOrientation.Below))
            {
                return Tileable.CellPosition.TopIsolated;
            }

            if (occupiedCellMap.HasOnly(CellOrientation.Left))
            {
                return Tileable.CellPosition.RightIsolated;
            }

            // Corners
            if (occupiedCellMap.HasAll(new[] { CellOrientation.Above, CellOrientation.AboveLeft, CellOrientation.Left }))
            {
                return Tileable.CellPosition.BottomRight;
            }

            if (occupiedCellMap.HasAll(new[] { CellOrientation.Above, CellOrientation.AboveRight, CellOrientation.Right }))
            {
                return Tileable.CellPosition.BottomLeft;
            }

            if (occupiedCellMap.HasAll(new[] { CellOrientation.Below, CellOrientation.BelowLeft, CellOrientation.Left }))
            {
                return Tileable.CellPosition.TopRight;
            }

            if (occupiedCellMap.HasAll(new[] { CellOrientation.Below, CellOrientation.BelowRight, CellOrientation.Right }))
            {
                return Tileable.CellPosition.TopLeft;
            }

            // Middle parts
            if (occupiedCellMap.Has(CellOrientation.Left))
            {
                return Tileable.CellPosition.Right;
            }

            if (occupiedCellMap.Has(CellOrientation.Right))
            {
                return Tileable.CellPosition.Left;
            }

            if (occupiedCellMap.Has(CellOrientation.Above))
            {
                return Tileable.CellPosition.Bottom;
            }

            if (occupiedCellMap.Has(CellOrientation.Below))
            {
                return Tileable.CellPosition.Top;
            }

            return Tileable.CellPosition.Single;
        }
    }
}