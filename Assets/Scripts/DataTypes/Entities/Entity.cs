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
        public EntityValidator buildValidator { get; }
        public EntityValidator destroyValidator { get; }

        public string typeLabel => Constants.TypeLabels.ValueFromKey(this.GetType());

        public bool canBuild => buildValidator.isValid;
        public bool canDestroy => destroyValidator.isValid;

        public Entity(EntityDefinition definition)
        {
            this.id = UIDGenerator.Generate(idKey);
            this.definition = definition;

            this.buildValidator = definition.buildValidatorFactory(this);
            this.destroyValidator = definition.destroyValidatorFactory(this);
        }

        public override string ToString() => $"{typeLabel}/{definition.title} {id}";

        public virtual void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public virtual void OnDestroy() { }
        public virtual void Setup() { }
        public virtual void Teardown() { }

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