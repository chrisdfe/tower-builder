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

        public int price => definition.pricePerCell * relativeCellCoordinatesList.Count;

        public bool isInBlueprintMode { get; set; } = false;

        public EntityGroup parent { get; set; } = null;

        public CellCoordinatesBlockList blocksList { get; private set; } = new CellCoordinatesBlockList();

        public CellCoordinates offsetCoordinates { get; set; } = CellCoordinates.zero;
        // TODO - absolute offset coordiantes?

        public CellCoordinatesList relativeCellCoordinatesList { get; private set; } = new CellCoordinatesList();

        // TODO - take parent into account
        public CellCoordinatesList absoluteCellCoordinatesList =>
            new CellCoordinatesList(
                relativeCellCoordinatesList.items
                    .Select(cellCoordinates =>
                    {
                        CellCoordinates result = cellCoordinates.Add(offsetCoordinates);
                        if (parent != null)
                        {
                            result = CellCoordinates.Add(result, parent.absoluteCellCoordinates);
                        }
                        return result;
                    }).ToList()
            );

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

            this.relativeCellCoordinatesList = definition.blockCellsTemplate.Clone();

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
            validator.Validate(appState);
        }

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            this.blocksList = EntityBlocksBuilder.FromDefinition(definition).Calculate(selectionBox);
            this.relativeCellCoordinatesList = CellCoordinatesList.FromBlocksList(this.blocksList);

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

        public CellCoordinatesBlock FindBlockByCellCoordinates(CellCoordinates cellCoordinates) =>
            blocksList.items.Find(cellCoordinatesBlock => cellCoordinatesBlock.Contains(cellCoordinates));

        /*
            Static Interface
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