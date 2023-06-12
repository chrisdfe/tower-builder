using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable, IValidatable
    {
        public virtual string idKey { get => "entity"; }

        public int id { get; }

        public int price => definition.pricePerCell * cellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        public EntityGroup parent { get; set; } = null;

        public CellCoordinatesList cellCoordinatesList { get; private set; } = new CellCoordinatesList();
        public CellCoordinatesBlockList blocksList { get; private set; } = new CellCoordinatesBlockList();

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

        public EntityDefinition definition { get; }
        protected EntityValidator validator { get; }

        public delegate EntityDefinition DefaultDefinitionGenerator();

        public virtual string typeLabel => "Entity";

        public bool isValid => validator.isValid;
        public ListWrapper<ValidationError> validationErrors => validator.errors;

        public Entity(EntityDefinition definition)
        {
            this.id = UIDGenerator.Generate(idKey);
            this.definition = definition;

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

        public void Validate(AppState appState)
        {
            Debug.Log("validating: ");
            validator.Validate(appState);
            Debug.Log(validationErrors.Count);

            validationErrors.items.ForEach(error =>
            {
                Debug.Log(error.message);
            });
        }

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            this.blocksList = EntityBlocksBuilder.FromDefinition(definition).Calculate(selectionBox);
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
            System.Type EntityType = Constants.EntityTypeEntityDefinitionMap.KeyFromValue(definition.GetType());

            if (EntityType != null)
            {
                return (Entity)(Activator.CreateInstance(EntityType, definition));
            }

            return null;
        }
    }
}