using System.Collections.Generic;
using System.Linq;
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
            BottomLeft, BottomRight
        }

        // Map of CellPosition enum -> fbx node names
        public static EnumStringMap<Tileable.CellPosition> CellPositionLabelMap = new EnumStringMap<Tileable.CellPosition>(
            new Dictionary<CellPosition, string>() {
                { CellPosition.Single, "Single" },

                { CellPosition.Left, "Left" },
                { CellPosition.HorizontalCenter, "HorizontalCenter" },
                { CellPosition.Right, "Right" },

                { CellPosition.Top, "Top" },
                { CellPosition.VerticalCenter, "VerticalCenter" },
                { CellPosition.Bottom, "Bottom" },

                { CellPosition.TopLeft, "TopLeft" },
                { CellPosition.TopRight, "TopRight" },
                { CellPosition.Center, "Center" },
                { CellPosition.BottomLeft, "BottomLeft" },
                { CellPosition.BottomRight, "BottomRight" },
            }
        );

        // Cell position in relation to another cell
        // TODO - put this in CellCoordinates?
        public enum CellOrientation
        {
            None,
            Above,
            AboveRight,
            Right,
            BelowRight,
            Below,
            BelowLeft,
            Left,
            AboveLeft
        }

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

        public static List<(CellCoordinates, Tileable.OccupiedCellMap)> CreateOccupiedCellMaps(CellCoordinatesList cellCoordinatesList) =>
            cellCoordinatesList.items.Select((cellCoordinates) =>
                (
                    cellCoordinates,
                    Tileable.OccupiedCellMap.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList)
                )
            ).ToList();

        public class OccupiedCellMap
        {
            public Dictionary<CellOrientation, bool> map { get; private set; } = new Dictionary<CellOrientation, bool>();

            public OccupiedCellMap(Dictionary<CellOrientation, bool> map)
            {
                this.map = map;
            }

            public bool Has(CellOrientation cellOrientation) => map.ContainsKey(cellOrientation) && map[cellOrientation];

            public bool HasAll(CellOrientation[] cellOrientations) =>
                GetOccupiedCells(cellOrientations).Count == cellOrientations.Count();

            public bool HasSome(CellOrientation[] cellOrientations) =>
                GetOccupiedCells(cellOrientations).Count > 0;

            public bool HasNone(CellOrientation[] cellOrientations) =>
                GetOccupiedCells(cellOrientations).Count == 0;

            List<CellOrientation> GetOccupiedCells(CellOrientation[] cellOrientations) =>
                cellOrientations.ToList().FindAll(orientation => Has(orientation));

            /*
                Static API
            */
            public static OccupiedCellMap FromCellCoordinatesList(CellCoordinates cellCoordinates, CellCoordinatesList cellCoordinatesList) =>
                new OccupiedCellMap(
                    new Dictionary<CellOrientation, bool>() {
                        { CellOrientation.Above,      cellCoordinatesList.Contains(cellCoordinates.coordinatesAbove) },
                        { CellOrientation.AboveRight, cellCoordinatesList.Contains(cellCoordinates.coordinatesAboveRight) },
                        { CellOrientation.Right,      cellCoordinatesList.Contains(cellCoordinates.coordinatesRight) },
                        { CellOrientation.BelowRight, cellCoordinatesList.Contains(cellCoordinates.coordinatesBelowRight) },
                        { CellOrientation.Below,      cellCoordinatesList.Contains(cellCoordinates.coordinatesBelow) },
                        { CellOrientation.BelowLeft,  cellCoordinatesList.Contains(cellCoordinates.coordinatesBelowLeft) },
                        { CellOrientation.Left,       cellCoordinatesList.Contains(cellCoordinates.coordinatesLeft) },
                        { CellOrientation.AboveLeft,  cellCoordinatesList.Contains(cellCoordinates.coordinatesAboveLeft) },
                    }
                );
        }
    }

    public class SingleTileable : Tileable
    {
        public override Type type { get; } = Type.Single;
        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied) => CellPosition.Single;

        public override CellPosition[] allPossibleCellPositions
        {
            get => new CellPosition[] { CellPosition.Single };
        }
    }

    public class HorizontalTileable : Tileable
    {
        public override Type type { get; } = Type.Horizontal;

        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.Left,
                    CellPosition.HorizontalCenter,
                    CellPosition.Right
                };
        }

        public override CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (occupied.HasAll(new CellOrientation[] { CellOrientation.Left, CellOrientation.Right }))
            {
                return CellPosition.HorizontalCenter;
            }

            if (occupied.Has(CellOrientation.Left))
            {
                return CellPosition.Right;
            }

            if (occupied.Has(CellOrientation.Right))
            {
                return CellPosition.Left;
            }

            return CellPosition.Single;
        }
    }

    public class VerticalTileable : Tileable
    {
        public override Type type { get; } = Type.Vertical;


        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.Top,
                    CellPosition.VerticalCenter,
                    CellPosition.Bottom
                };
        }

        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (occupied.HasAll(new CellOrientation[] { CellOrientation.Above, CellOrientation.Below }))
            {
                return Tileable.CellPosition.VerticalCenter;
            }

            if (occupied.Has(CellOrientation.Above))
            {
                return Tileable.CellPosition.Bottom;
            }

            if (occupied.Has(CellOrientation.Below))
            {
                return Tileable.CellPosition.Top;
            }

            return Tileable.CellPosition.Single;
        }
    }

    public class DiagonalTileable : Tileable
    {
        public override Type type { get; } = Type.Diagonal;

        public override CellPosition[] allPossibleCellPositions
        {
            get =>
                new CellPosition[] {
                    CellPosition.BottomLeft,
                    CellPosition.DiagonalCenter,
                    CellPosition.TopRight
                };
        }

        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (occupied.HasAll(new CellOrientation[] { CellOrientation.BelowLeft, CellOrientation.AboveRight }))
            {
                return Tileable.CellPosition.DiagonalCenter;
            }

            if (occupied.Has(CellOrientation.AboveRight))
            {
                return Tileable.CellPosition.BottomLeft;
            }

            if (occupied.Has(CellOrientation.BelowLeft))
            {
                return Tileable.CellPosition.TopRight;
            }

            return Tileable.CellPosition.Single;
        }
    }

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
                    CellPosition.BottomRight
                };
        }

        public override Tileable.CellPosition GetCellPosition(OccupiedCellMap occupied)
        {
            if (
                occupied.HasAll(new CellOrientation[] {
                    CellOrientation.Above,
                    CellOrientation.Right,
                    CellOrientation.Below,
                    CellOrientation.Left
                })
            )
            {
                return Tileable.CellPosition.Center;
            }

            if (occupied.HasAll(new CellOrientation[] { CellOrientation.Above, CellOrientation.Below }))
            {
                return Tileable.CellPosition.VerticalCenter;
            }

            if (occupied.HasAll(new CellOrientation[] { CellOrientation.Left, CellOrientation.Right }))
            {
                return Tileable.CellPosition.HorizontalCenter;
            }

            // TODO - walls
            // TODO - corners

            if (occupied.Has(CellOrientation.Left))
            {
                return Tileable.CellPosition.Right;
            }

            if (occupied.Has(CellOrientation.Right))
            {
                return Tileable.CellPosition.Left;
            }

            if (occupied.Has(CellOrientation.Above))
            {
                return Tileable.CellPosition.Bottom;
            }

            if (occupied.Has(CellOrientation.Below))
            {
                return Tileable.CellPosition.Top;
            }


            return Tileable.CellPosition.Single;
        }
    }
}