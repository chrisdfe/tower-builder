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
        // TODO - put elsewhere
        public static EnumStringMap<Type> TypeLabels = new EnumStringMap<Type>(
            new Dictionary<Type, string>() {
                { typeof(Floor),              "Floor" },
                { typeof(InteriorWall),       "InteriorWall" },
                { typeof(InteriorLight),      "InteriorLight" },
                { typeof(Resident),           "Resident" },
                { typeof(Furniture),          "Furniture" },
                { typeof(TransportationItem), "Transportation Item" },
                { typeof(FreightItem),        "Freight" },
                { typeof(Wheel),              "Wheel" },
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

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            EntityBlocksCalculatorBase blocksCalculator = EntityBlocksCalculator.FromDefinition(definition);

            this.blocksList = blocksCalculator.CalculateFromSelectionBox(selectionBox);
            // TODO 
            // 1) put this somewhere not inside of Entity
            // 2) use a different origin than bottomLeftCoordinates
            this.blocksList.PositionAtCoordinates(selectionBox.cellCoordinatesList.bottomLeftCoordinates);

            this.cellCoordinatesList = CellCoordinatesList.FromBlocksList(this.blocksList);


            CalculateTileableMap();
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