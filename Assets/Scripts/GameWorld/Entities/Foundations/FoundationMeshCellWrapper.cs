using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class FoundationMeshCellWrapper : EntityMeshCellWrapper
    {
        const string CEILING_NODE_NAME = "Ceiling";
        const string LEFT_WALL_NODE_NAME_BASE = "LeftWall";
        const string RIGHT_WALL_NODE_NAME_BASE = "RightWall";
        const string BACK_WALL_NODE_NAME_BASE = "BackWall";
        const string FLOOR_NODE_NAME_BASE = "Floor";

        enum Segment
        {
            Ceiling,
            LeftWall,
            RightWall,
            BackWall,
            Floor
        }

        static EnumStringMap<Segment> SegmentNodeNameMap = new EnumStringMap<Segment>(
            new Dictionary<Segment, string>() {
                { Segment.Ceiling,   CEILING_NODE_NAME },
                { Segment.LeftWall,  LEFT_WALL_NODE_NAME_BASE },
                { Segment.RightWall, RIGHT_WALL_NODE_NAME_BASE },
                { Segment.BackWall,  BACK_WALL_NODE_NAME_BASE },
                { Segment.Floor,     FLOOR_NODE_NAME_BASE },
            }
        );

        static Dictionary<Tileable.CellPosition, Segment[]> SEGMENTS_FOR_CELL_POSITION_MAP =
            new Dictionary<Tileable.CellPosition, Segment[]>() {
                {
                    Tileable.CellPosition.None,
                    new Segment[] { }
                },

                // Single (isolated)
                {
                    Tileable.CellPosition.Single,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall,
                        Segment.RightWall,
                        Segment.Floor
                    }
                },
                // middle walls
                {
                    Tileable.CellPosition.Top,
                    new Segment[] {
                        Segment.Ceiling
                    }
                },
                {
                    Tileable.CellPosition.Right,
                    new Segment[] {
                        Segment.RightWall
                    }
                },
                {
                    Tileable.CellPosition.Bottom,
                    new Segment[] {
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.Left,
                    new Segment[] {
                        Segment.LeftWall
                    }
                },
                // Centers
                {
                    Tileable.CellPosition.Center,
                    new Segment[] { }
                },
                {
                    Tileable.CellPosition.HorizontalCenter,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.VerticalCenter,
                    new Segment[] {
                        Segment.LeftWall,
                        Segment.RightWall
                    }
                },
                {
                    Tileable.CellPosition.LowToHighDiagonalCenter,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall,
                        Segment.RightWall,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.HighToLowDiagonalCenter,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall,
                        Segment.RightWall,
                        Segment.Floor
                    }
                },

                // corners
                {
                    Tileable.CellPosition.TopLeft,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall
                    }
                },
                {
                    Tileable.CellPosition.TopRight,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.RightWall
                    }
                },
                {
                    Tileable.CellPosition.BottomRight,
                    new Segment[] {
                        Segment.RightWall,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.BottomLeft,
                    new Segment[] {
                        Segment.LeftWall,
                        Segment.Floor
                    }
                },

                // Isolated cells
                {
                    Tileable.CellPosition.TopIsolated,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall,
                        Segment.RightWall,
                    }
                },
                {
                    Tileable.CellPosition.RightIsolated,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.RightWall,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.BottomIsolated,
                    new Segment[] {
                        Segment.LeftWall,
                        Segment.RightWall,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.LeftIsolated,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall,
                        Segment.Floor
                    }
                },

                {
                    Tileable.CellPosition.TopRightIsolated,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.RightWall,
                    }
                },
                {
                    Tileable.CellPosition.BottomRightIsolated,
                    new Segment[] {
                        Segment.RightWall,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.BottomLeftIsolated,
                    new Segment[] {
                        Segment.LeftWall,
                        Segment.Floor
                    }
                },
                {
                    Tileable.CellPosition.TopLeftIsolated,
                    new Segment[] {
                        Segment.Ceiling,
                        Segment.LeftWall
                    }
                },
            };

        EnumMap<Segment, Transform> segmentTransformMap;

        public FoundationMeshCellWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellCoordinates relativeCellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) : base(parent, prefabMesh, cellCoordinates, relativeCellCoordinates, cellNeighbors, cellPosition)
        {
        }

        protected override void ProcessModel()
        {
            base.ProcessModel();


            BuildSegmentTransformMap();
            ToggleSegmentsForCellPosition();
        }

        void BuildSegmentTransformMap()
        {
            Dictionary<Segment, Transform> input = new Dictionary<Segment, Transform>();
            foreach (Segment segment in Enum.GetValues(typeof(Segment)))
            {
                input.Add(segment, tileabileWrapper.Find(SegmentNodeNameMap.ValueFromKey(segment)));
            }

            segmentTransformMap = new EnumMap<Segment, Transform>(input);
        }

        void ToggleSegmentsForCellPosition()
        {
            List<Segment> currentSegments = SEGMENTS_FOR_CELL_POSITION_MAP[cellPosition].ToList();

            foreach (Segment segment in segmentTransformMap.keys)
            {
                Transform segmentTransform = segmentTransformMap.ValueFromKey(segment);

                // Only render the back wall if there is no window there
                // (for now)
                if (segment == Segment.BackWall)
                {
                    Entity windowAtCell = Registry.appState.Entities.Windows.queries.FindEntityAtCell(cellCoordinates);

                    segmentTransform.gameObject.SetActive(windowAtCell == null);
                }
                else
                {
                    segmentTransform.gameObject.SetActive(currentSegments.Contains(segment));
                }

            }
        }
    }
}