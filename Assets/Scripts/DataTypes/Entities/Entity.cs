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
    public class Entity : ISetupable
    {
        public virtual string idKey => "entity";

        public int id { get; }

        public int price => definition.pricePerCell * relativeCellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        public CellCoordinates offsetCoordinates { get; set; } = CellCoordinates.zero;

        public CellCoordinatesBlockList relativeBlocksList { get; private set; } = new CellCoordinatesBlockList();

        public CellCoordinatesList relativeCellCoordinatesList =>
            CellCoordinatesList.FromBlocksList(this.relativeBlocksList);

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

        public EntityDefinition definition { get; }
        protected EntityValidator buildValidator { get; }
        protected EntityValidator destroyValidator { get; }

        public virtual string typeLabel => "Entity";

        public bool canBuild => buildValidator.isValid;
        public ListWrapper<ValidationError> buildValidationErrors => buildValidator.errors;

        public bool canDestroy => destroyValidator.isValid;
        public ListWrapper<ValidationError> destroyValidationErrors => buildValidator.errors;

        public Entity(EntityDefinition definition)
        {
            this.id = UIDGenerator.Generate(idKey);
            this.definition = definition;

            this.buildValidator = definition.buildValidatorFactory(this);
            this.destroyValidator = definition.destroyValidatorFactory(this);
        }

        public override string ToString() => $"{definition.title} #{id}";

        public virtual void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public virtual void OnDestroy() { }
        public virtual void Setup() { }
        public virtual void Teardown() { }

        public void ValidateBuild(AppState appState)
        {
            buildValidator.Validate(appState);
        }

        public void ValidateDestroy(AppState appState)
        {
            destroyValidator.Validate(appState);
        }

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            this.relativeBlocksList = EntityBlocksBuilder.FromDefinition(definition).Calculate(selectionBox);

            CalculateTileableMap();
        }

        public void CalculateTileableMap()
        {
            cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
            cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

            relativeCellCoordinatesList.ForEach((cellCoordinates) =>
            {
                CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, relativeCellCoordinatesList);
                cellNeighborsMap.Add(cellCoordinates, cellNeighbors);

                Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);
                cellPositionMap.Add(cellCoordinates, cellPosition);
            });
        }

        /*
            Static Interface
        */
        public static Entity CreateFromDefinition(EntityDefinition definition)
        {
            Type EntityType = Constants.EntityTypeEntityDefinitionMap.KeyFromValue(definition.GetType());

            if (EntityType != null)
            {
                return (Entity)(Activator.CreateInstance(EntityType, definition));
            }

            return null;
        }
    }
}