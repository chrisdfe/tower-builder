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

        public static EnumStringMap<Type> TypeLabels = new EnumStringMap<Type>(
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
            Diagonal,
            Flexible,
        }

        public int price => definition.pricePerCell * cellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>() { };
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>() { };

        // TODO - remove the set; accessors too
        public CellCoordinatesList cellCoordinatesList { get; set; } = new CellCoordinatesList();
        public CellCoordinatesBlockList blocksList { get; set; } = new CellCoordinatesBlockList();

        public EntityDefinition definition { get; }

        // TODO - remove this and put in seperate state - only have list of error messages or isValid
        public EntityValidator validator { get; }


        public Entity(EntityDefinition definition)
        {

            this.definition = definition;
            this.id = UIDGenerator.Generate(idKey);

            this.cellCoordinatesList = definition.cellCoordinatesList.Clone();

            this.validator = definition.validatorFactory(this);
        }

        public override string ToString() => $"{definition.title} #{id}";

        public virtual void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public virtual void OnDestroy() { }

        public virtual void Setup() { }
        public virtual void Teardown() { }

        public virtual void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            cellCoordinatesList.PositionAtCoordinates(cellCoordinates);
        }

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            CreateBlockCells();
            // PositionAtCoordinates(GetBottomLeftCoordinates());
            PositionAtCoordinates(selectionBox.cellCoordinatesList.bottomLeftCoordinates.Clone());
            SetTileableMap();

            void CreateBlockCells()
            {
                List<CellCoordinates> blockStartingCoordinates = GetBlockStartingCoordinates();
                CellCoordinatesList newCellCoordinatesList = new CellCoordinatesList();
                CellCoordinatesBlockList newBlocksList = new CellCoordinatesBlockList();

                foreach (CellCoordinates startingCoordinates in blockStartingCoordinates)
                {
                    CellCoordinatesList blockCells = definition.cellCoordinatesList.Clone();
                    blockCells.PositionAtCoordinates(startingCoordinates);

                    newCellCoordinatesList.Add(blockCells);

                    newBlocksList.Add(
                        blockCells.items.Select(cellCoordinates => new CellCoordinatesBlock(cellCoordinates)).ToList()
                    );
                }

                Debug.Log("newCellCoordinatesList");
                Debug.Log(newCellCoordinatesList.Count);

                this.cellCoordinatesList = newCellCoordinatesList;
                this.blocksList = newBlocksList;
            }

            void SetTileableMap()
            {
                cellCoordinatesList.ForEach((cellCoordinates) =>
                {
                    CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList);
                    cellNeighborsMap.Add(cellCoordinates, cellNeighbors);

                    Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);
                    cellPositionMap.Add(cellCoordinates, cellPosition);
                });
            }

            List<CellCoordinates> GetBlockStartingCoordinates()
            {
                List<CellCoordinates> result = new List<CellCoordinates>();

                switch (definition.resizability)
                {
                    case Resizability.Horizontal:
                        CalculateHorizontal();
                        break;
                    case Resizability.Vertical:
                        CalculateVertical();
                        break;
                    case Resizability.Flexible:
                        CalculateFlexible();
                        break;
                    case Resizability.Diagonal:
                        CalculateDiagonal();
                        break;
                    case Resizability.Inflexible:
                        result.Add(CellCoordinates.zero);
                        break;
                }

                return result;

                void CalculateFlexible()
                {
                    for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += definition.cellCoordinatesList.width)
                    {
                        for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += definition.cellCoordinatesList.floorSpan)
                        {
                            result.Add(new CellCoordinates(x, floor));
                        }
                    }
                }

                void CalculateHorizontal()
                {
                    for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += definition.cellCoordinatesList.width)
                    {
                        result.Add(new CellCoordinates(x, 0));
                    }
                }

                void CalculateVertical()
                {
                    for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += definition.cellCoordinatesList.floorSpan)
                    {
                        result.Add(new CellCoordinates(0, floor));
                    }
                }

                void CalculateDiagonal()
                {
                    int x = 0;
                    int floor = 0;

                    while (x < selectionBox.cellCoordinatesList.width && floor < selectionBox.cellCoordinatesList.floorSpan)
                    {
                        result.Add(new CellCoordinates(x, floor));
                        x += definition.cellCoordinatesList.width;
                        floor += definition.cellCoordinatesList.width;
                    }
                }
            }

            CellCoordinates GetBottomLeftCoordinates() =>
                definition.resizability switch
                {
                    Resizability.Flexible =>
                        new CellCoordinates(
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.x,
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.floor
                        ),
                    Resizability.Diagonal =>
                        selectionBox.cellCoordinatesList.bottomLeftCoordinates,
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

        public Entity(EntityDefinition<KeyType> definition) : base(definition as EntityDefinition)
        {
            this.key = definition.key;
        }
    }
}