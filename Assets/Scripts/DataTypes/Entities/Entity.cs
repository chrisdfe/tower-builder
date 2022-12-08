using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable
    {
        public virtual string idKey { get => "entity"; }

        public int id { get; }

        public enum Type
        {
            Room,
            Resident,
            Furniture,
            TransportationItem
        }

        static EnumStringMap<Type> TypeLabels = new EnumStringMap<Type>(
            new Dictionary<Type, string>() {
                { Type.Room,               "Room" },
                { Type.Resident,           "Resident" },
                { Type.Furniture,          "Furniture" },
                { Type.TransportationItem, "Transportation Item" }
            }
        );

        public enum Resizability
        {
            Inflexible,
            Horizontal,
            Vertical,
            Flexible,
        }

        public int price => template.pricePerCell * cellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        // TODO - I don't know if I need all of these
        public Dictionary<CellCoordinates, OccupiedCellMap> occupiedCellMapMap = new Dictionary<CellCoordinates, OccupiedCellMap>() { };
        public Dictionary<CellCoordinates, Tileable> tileableMap = new Dictionary<CellCoordinates, Tileable>() { };
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>() { };

        // TODO - remove the set; accessors too
        public CellCoordinatesList cellCoordinatesList { get; set; } = new CellCoordinatesList();
        public CellCoordinatesBlockList blocksList { get; set; } = new CellCoordinatesBlockList();

        public EntityTemplate template { get; }

        public Entity(EntityTemplate template)
        {
            this.template = template;
            this.id = UIDGenerator.Generate(idKey);

            this.cellCoordinatesList = template.cellCoordinatesList.Clone();
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void OnDestroy() { }
        public virtual void Setup() { }
        public virtual void Teardown() { }

        public virtual void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            cellCoordinatesList.PositionAtCoordinates(cellCoordinates);
        }

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            CreateBlockCells();
            PositionAtCoordinates(GetBottomLeftCoordinates());
            SetTileableMap();

            void CreateBlockCells()
            {
                Dimensions blockCount = GetBlockCount();
                CellCoordinatesList cellCoordinatesList = new CellCoordinatesList();
                CellCoordinatesBlockList blocksList = new CellCoordinatesBlockList();

                Debug.Log("blockCount: " + blockCount);

                for (int x = 0; x < blockCount.width; x++)
                {
                    for (int floor = 0; floor < blockCount.height; floor++)
                    {
                        CellCoordinatesList blockCells = CellCoordinatesList.CreateRectangle(template.blockSize.width, template.blockSize.height);
                        blockCells.PositionAtCoordinates(new CellCoordinates(x, floor));

                        cellCoordinatesList.Add(blockCells);

                        if (template.blockSize.Matches(Dimensions.one))
                        {
                            blocksList.Add(
                                blockCells.items.Select(cellCoordinates => new CellCoordinatesBlock(cellCoordinates)).ToList()
                            );
                        }
                        else
                        {
                            blocksList.Add(new CellCoordinatesBlock(blockCells));
                        }
                    }
                }

                this.cellCoordinatesList = cellCoordinatesList;
                this.blocksList = blocksList;
            }

            void SetTileableMap()
            {
                Debug.Log("SetTileableMap");
                Debug.Log("cellCoordinatesList");
                Debug.Log(cellCoordinatesList.Count);

                cellCoordinatesList.ForEach((cellCoordinates) =>
                {
                    OccupiedCellMap occupiedCellMap = OccupiedCellMap.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList);
                    occupiedCellMapMap.Add(cellCoordinates, occupiedCellMap);

                    Tileable tileable = Tileable.FromResizability(template.resizability);
                    tileableMap.Add(cellCoordinates, tileable);

                    Tileable.CellPosition cellPosition = tileable.GetCellPosition(occupiedCellMap);
                    Debug.Log("cellPosition: " + cellPosition);

                    cellPositionMap.Add(cellCoordinates, cellPosition);
                });
            }

            Dimensions GetBlockCount() =>
                template.resizability switch
                {
                    Resizability.Flexible =>
                        new Dimensions(
                            MathUtils.RoundUpToNearest(selectionBox.dimensions.width, template.blockSize.width),
                            MathUtils.RoundUpToNearest(selectionBox.dimensions.height, template.blockSize.height)
                        ),
                    Resizability.Horizontal =>
                        new Dimensions(
                            MathUtils.RoundUpToNearest(selectionBox.dimensions.width, template.blockSize.width),
                            1
                        ),
                    Resizability.Vertical =>
                        new Dimensions(
                            1,
                            MathUtils.RoundUpToNearest(selectionBox.dimensions.height, template.blockSize.height)
                        ),
                    Resizability.Inflexible => Dimensions.one,
                    _ => Dimensions.one,
                };

            CellCoordinates GetBottomLeftCoordinates() =>
                template.resizability switch
                {
                    Resizability.Flexible =>
                        new CellCoordinates(
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.x,
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.floor
                        ),
                    Resizability.Horizontal =>
                        new CellCoordinates(
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.x,
                            selectionBox.start.floor
                        ),
                    Resizability.Vertical =>
                         new CellCoordinates(
                            selectionBox.start.x,
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.floor
                        ),
                    Resizability.Inflexible => selectionBox.end,
                    _ => selectionBox.end
                };
        }

        public CellCoordinatesBlock FindBlockByCellCoordinates(CellCoordinates cellCoordinates) =>
            blocksList.items.Find(cellCoordinatesBlock => cellCoordinatesBlock.Contains(cellCoordinates));
    }

    public class Entity<KeyType> : Entity
        where KeyType : struct
    {
        public KeyType key { get; protected set; }

        public Entity(EntityTemplate<KeyType> template) : base(template as EntityTemplate)
        {
            this.key = template.key;
        }
    }
}