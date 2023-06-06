using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable
    {
        public virtual string idKey { get => "entity"; }

        public int id { get; }

        public int[] zLayers = new int[] { 0 };

        public int price => definition.pricePerCell * cellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

        // TODO - remove the set; accessors too
        public CellCoordinatesList cellCoordinatesList { get; set; } = new CellCoordinatesList();
        public CellCoordinatesBlockList blocksList { get; set; } = new CellCoordinatesBlockList();

        public EntityDefinition definition { get; }

        // TODO - remove this and put in seperate state - only have list of error messages or isValid
        public EntityValidator validator { get; }

        public string typeLabel => Constants.GetEntityDefinitionLabel(definition);

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