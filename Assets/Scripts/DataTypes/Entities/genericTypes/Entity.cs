using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Entities.InteriorWalls;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
// using TowerBuilder.DataTypes.Entities.Rooms;
// using TowerBuilder.DataTypes.Entities.Vehicles;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Entities.Windows;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable
    {
        public static EnumStringMap<Type> TypeLabels = new EnumStringMap<Type>(
            new Dictionary<Type, string>() {
                // { typeof(Room),               "Room" },
                { typeof(Floor),              "Floor" },
                { typeof(InteriorWall),       "InteriorWall" },
                { typeof(InteriorLight),      "InteriorLight" },
                { typeof(Resident),           "Resident" },
                { typeof(Furniture),          "Furniture" },
                { typeof(TransportationItem), "Transportation Item" },
                { typeof(FreightItem),        "Freight" },
                { typeof(Wheel),              "Wheel" },
                // { typeof(Vehicle),            "Vehicle" },
                { typeof(Window),             "Window" }
            }
        );

        public virtual string idKey { get => "entity"; }

        public int id { get; }

        public int[] zLayers = new int[] { 0 };

        public int price => definition.pricePerCell * cellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        public string typeLabel => TypeLabels.ValueFromKey(GetType());

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

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

            this.cellCoordinatesList = definition.blockCellsTemplate.Clone();

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
            CalculateTileableMap();

            void CreateBlockCells()
            {
                CellCoordinatesList blockStartingCoordinates = new CellCoordinatesList(GetBlockStartingCoordinates());

                blockStartingCoordinates.PositionAtCoordinates(selectionBox.cellCoordinatesList.bottomLeftCoordinates.Clone());

                CellCoordinatesList newCellCoordinatesList = new CellCoordinatesList();
                CellCoordinatesBlockList newBlocksList = new CellCoordinatesBlockList();

                foreach (CellCoordinates startingCoordinates in blockStartingCoordinates.items)
                {
                    CellCoordinatesBlock blockCells = new CellCoordinatesBlock(definition.blockCellsTemplate.Clone());
                    blockCells.PositionAtCoordinates(startingCoordinates);

                    newCellCoordinatesList.Add(blockCells);

                    newBlocksList.Add(blockCells);
                }

                this.cellCoordinatesList = newCellCoordinatesList;
                this.blocksList = newBlocksList;
            }

            List<CellCoordinates> GetBlockStartingCoordinates()
            {
                List<CellCoordinates> result = new List<CellCoordinates>();
                Distance incrementAmount = new Distance(
                    definition.staticBlockSize ? definition.blockCellsTemplate.width : 1,
                    definition.staticBlockSize ? definition.blockCellsTemplate.floorSpan : 1
                );

                Debug.Log(definition.resizability);

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
                    for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
                    {
                        for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += incrementAmount.floors)
                        {
                            result.Add(new CellCoordinates(x, floor));
                        }
                    }
                }

                void CalculateHorizontal()
                {
                    for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
                    {
                        result.Add(new CellCoordinates(x, 0));
                    }
                }

                void CalculateVertical()
                {
                    for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += incrementAmount.floors)
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
                        x += incrementAmount.x;
                        floor += incrementAmount.floors;
                    }
                }
            }
        }

        public void CalculateTileableMap()
        {
            cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
            cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

            cellCoordinatesList.ForEach((cellCoordinates) =>
            {
                CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList);
                cellNeighborsMap.Add(cellCoordinates, cellNeighbors);

                Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);
                cellPositionMap.Add(cellCoordinates, cellPosition);
            });
        }

        public CellCoordinatesBlock FindBlockByCellCoordinates(CellCoordinates cellCoordinates) =>
            blocksList.items.Find(cellCoordinatesBlock => cellCoordinatesBlock.Contains(cellCoordinates));

        /*
            Static API
        */
        public static Entity CreateFromDefinition(EntityDefinition definition)
        {
            System.Type EntityType = EntityDefinitions.EntityDefinitionEntityTypeMap[definition.GetType()];

            if (EntityType != null)
            {
                return (Entity)(Activator.CreateInstance(EntityType, definition));
            }

            return null;
        }

        public static string GetEntityDefinitionLabel(EntityDefinition definition) =>
            definition switch
            {
                // RoomDefinition roomDefinition =>
                //     Room.KeyLabelMap.ValueFromKey(roomDefinition.key),
                FloorDefinition floorDefinition =>
                    Floor.KeyLabelMap.ValueFromKey(floorDefinition.key),
                InteriorWallDefinition interiorWallDefinition =>
                    InteriorWall.KeyLabelMap.ValueFromKey(interiorWallDefinition.key),
                FurnitureDefinition furnitureDefinition =>
                    Furniture.KeyLabelMap.ValueFromKey(furnitureDefinition.key),
                ResidentDefinition residentDefinition =>
                    Resident.KeyLabelMap.ValueFromKey(residentDefinition.key),
                TransportationItemDefinition transportationItemDefinition =>
                    TransportationItem.KeyLabelMap.ValueFromKey(transportationItemDefinition.key),
                FreightDefinition freightDefinition =>
                    FreightItem.KeyLabelMap.ValueFromKey(freightDefinition.key),
                _ => null
            };
    }

    public class Entity<KeyType> : Entity
        where KeyType : struct
    {
        public KeyType key { get; protected set; }

        // public static EnumStringMap<KeyType> KeyLabelMap;

        public Entity(EntityDefinition<KeyType> definition) : base(definition as EntityDefinition)
        {
            this.key = definition.key;
        }
    }
}