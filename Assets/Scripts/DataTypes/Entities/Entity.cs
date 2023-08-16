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

            public Input(object rawInput) : base(rawInput)
            {
                Dictionary<string, object> castRawInput = (Dictionary<string, object>)rawInput;

                this.id = (int)castRawInput["id"];
                this.relativeOffsetCoordinates = new CellCoordinates.Input(castRawInput["relativeOffsetCoordinates"]);
                this.relativeBlocksList = new CellCoordinatesBlockList.Input(castRawInput["relativeBlocksList"]);
                this.definition = new EntityDefinition.Input(castRawInput["definition"]);
            }

            public override object ToRawInput() =>
                new Dictionary<string, object>()
                {
                    { "id", this.id },
                    { "typeLabel", typeLabel },
                    { "relativeOffsetCoordinates", relativeOffsetCoordinates.ToRawInput() },
                    { "relativeBlocksList", this.relativeBlocksList.ToRawInput() },
                    { "definition", this.definition.ToRawInput() }
                };
        }

        public virtual string idKey => "entity";

        public int id { get; private set; }

        // Relative to parent's bottomLeftCoordinates
        public CellCoordinates relativeOffsetCoordinates { get; set; } = CellCoordinates.zero;

        public CellCoordinatesBlockList relativeBlocksList { get; private set; } = new CellCoordinatesBlockList();

        public CellCoordinatesList relativeCellCoordinatesList =>
            CellCoordinatesList.FromBlocksList(this.relativeBlocksList);

        public Dictionary<CellCoordinates, CellNeighbors> cellNeighborsMap = new Dictionary<CellCoordinates, CellNeighbors>();
        public Dictionary<CellCoordinates, Tileable.CellPosition> cellPositionMap = new Dictionary<CellCoordinates, Tileable.CellPosition>();

        public EntityDefinition definition { get; private set; }

        public EntityValidator buildValidator { get; private set; }
        public EntityValidator destroyValidator { get; private set; }

        public EntityTypeData entityTypeData => EntityTypeData.Get(this.GetType());

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
            this.id = UIDGenerator.Generate(idKey);
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

            };

        public void ConsumeInput(SaveableInputBase baseInput)
        {
            Input input = (Input)baseInput;
            this.id = input.id;
            this.relativeOffsetCoordinates = new CellCoordinates(input.relativeOffsetCoordinates);
            this.relativeBlocksList = new CellCoordinatesBlockList(input.relativeBlocksList);
            this.definition = Entities.Definitions.FindDefinitionByInput(input.definition);
        }

        void PostConstruct()
        {
            this.buildValidator = definition.buildValidatorFactory(this);
            this.destroyValidator = definition.destroyValidatorFactory(this);
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
        public static Entity FromInput(Dictionary<string, object> input)
        {
            Debug.Log("Entity from input");
            Debug.Log(input);

            Debug.Log("input");
            Debug.Log(input);
            Debug.Log(input["id"]);
            Debug.Log(input["typeLabel"]);
            Debug.Log(input["relativeOffsetCoordinates"]);
            Debug.Log(input["relativeBlocksList"]);
            Debug.Log(input["definition"]);

            // Debug.Log("input.typeLabel");
            // Debug.Log(input.typeLabel);

            // EntityTypeData inputEntityType = EntityTypeData.FindByLabel(input.typeLabel);
            // Debug.Log("inputEntityType");
            // Debug.Log(inputEntityType);

            // Type EntityType = inputEntityType.EntityType;
            // Debug.Log("EntityType");
            // Debug.Log(EntityType);

            // if (EntityType != null)
            // {
            //     return (Entity)(Activator.CreateInstance(EntityType, input));
            // }

            return null;
        }

        public static Entity CreateFromDefinition(EntityDefinition definition)
        {
            EntityTypeData entityTypeData = EntityTypeData.FindByDefinition(definition);
            Type EntityType = entityTypeData.EntityType;

            if (EntityType != null)
            {
                return (Entity)(Activator.CreateInstance(EntityType, definition));
            }

            return null;
        }
    }
}