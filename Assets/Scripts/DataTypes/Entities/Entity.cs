using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.Definitions;
using TowerBuilder.Systems;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class Entity : ISetupable, ISaveable
    {
        public class Input : SaveableInputBase
        {
            public string typeLabel;
            public int id;
            public CellCoordinates.Input relativeOffsetCoordinates;
            public CellCoordinatesBlockList.Input relativeBlocksList;
            public EntityDefinition.Input definition;

            public Input() : base() { }
        }

        public virtual string idKey => "entity";

        public int id { get; private set; }

        // Relative to parent's bottomLeftCoordinates
        public CellCoordinates relativeOffsetCoordinates { get; set; } = CellCoordinates.zero;

        public CellCoordinatesBlockList relativeBlocksList { get; private set; } = new CellCoordinatesBlockList();

        public CellCoordinatesList relativeCellCoordinatesList =>
            CellCoordinatesList.FromBlocksList(relativeBlocksList);

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

        public EntityDefinition definition { get; private set; }

        public EntityValidator buildValidator { get; private set; }
        public EntityValidator destroyValidator { get; private set; }

        public EntityTypeData entityTypeData => EntityTypeData.Get(GetType());

        public string typeLabel => entityTypeData.label;

        // TODO - add "custom z Index"
        public int zIndex => entityTypeData.zIndex;

        public bool isInBlueprintMode { get; set; } = false;
        public bool isMarkedForDeletion { get; set; } = false;

        public int price => definition.pricePerCell * relativeCellCoordinatesList.Count;

        public override string ToString() => $"entity";

        public Entity() { }

        public Entity(EntityDefinition definition)
        {
            id = UIDGenerator.Generate(idKey);
            this.definition = definition;

            PostConstruct();
        }

        public Entity(SaveableInputBase input)
        {
            ConsumeInput(input);
            PostConstruct();
        }

        public SaveableInputBase ToInput() =>
            new Input()
            {
                typeLabel = EntityTypeData.Get(GetType()).label,
                id = id,
                relativeOffsetCoordinates = relativeOffsetCoordinates.ToInput() as CellCoordinates.Input,
                relativeBlocksList = relativeBlocksList.ToInput() as CellCoordinatesBlockList.Input,
                definition = definition.ToInput() as EntityDefinition.Input
            };

        public void ConsumeInput(SaveableInputBase baseInput)
        {
            Input input = (Input)baseInput;
            id = input.id;
            relativeOffsetCoordinates = new CellCoordinates(input.relativeOffsetCoordinates);
            relativeBlocksList = new CellCoordinatesBlockList(input.relativeBlocksList);
            definition = Definitions.FindDefinitionByInput(input.definition);
        }

        void PostConstruct()
        {
            buildValidator = definition.buildValidatorFactory(this);
            destroyValidator = definition.destroyValidatorFactory(this);
        }

        public virtual void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public virtual void OnDestroy() { }
        public virtual void Setup() { }
        public virtual void Teardown() { }

        public void CalculateCellsFromSelectionBox(SelectionBox selectionBox)
        {
            relativeBlocksList = EntityBlocksBuilderBase.FromDefinition(definition).CalculateFromSelectionBox(selectionBox);

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
        public static Entity FromInput(Input input)
        {
            Entity entity = new Entity();
            EntityTypeData newEntityTypeData = EntityTypeData.FindByLabel(input.typeLabel);
            Type EntityType = newEntityTypeData.EntityType;
            Entity newEntity = (Entity)Activator.CreateInstance(EntityType);
            newEntity.ConsumeInput(input);

            return newEntity;
        }

        public static Entity CreateFromDefinition(EntityDefinition definition)
        {
            EntityTypeData entityTypeData = EntityTypeData.FindByDefinition(definition);
            Type EntityType = entityTypeData.EntityType;

            if (EntityType != null)
            {
                return (Entity)Activator.CreateInstance(EntityType, definition);
            }

            return null;
        }
    }
}