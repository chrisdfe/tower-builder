using System;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public struct Input
        {
            public string selectedEntityCategory;
        }

        public class Events
        {
            public delegate void SelectedEntityKeyEvent(Type entityKey, Type previousEntityType);
            public SelectedEntityKeyEvent onSelectedEntityKeyUpdated;

            public delegate void buildIsActiveEvent();
            public buildIsActiveEvent onBuildStart;
            public buildIsActiveEvent onBuildEnd;

            public delegate void SelectedEntityCategoryEvent(string selectedEntityCategory);
            public SelectedEntityCategoryEvent onSelectedEntityCategoryUpdated;

            public delegate void SelectedEntityDefinitionEvent(EntityDefinition selectedEntityDefinition);
            public SelectedEntityDefinitionEvent onSelectedEntityDefinitionUpdated;

            public delegate void blueprintUpdateEvent(Entity blueprintEntity);
            public blueprintUpdateEvent onBlueprintEntityUpdated;
        }

        // public Type selectedEntityType { get; private set; } = typeof(DataTypes.EntityGroups.Rooms.Room);
        public Type selectedEntityType { get; private set; } = typeof(DataTypes.Entities.Floors.Floor);
        public string selectedEntityCategory { get; private set; } = "";
        public EntityDefinition selectedEntityDefinition { get; private set; } = null;
        public Entity blueprintEntity { get; private set; } = null;

        public Events events;

        public bool isLocked = false;
        bool buildIsActive = false;

        public BuildToolState(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
            events = new Events();

            ResetCategoryAndDefinition();

        }

        public override void Setup()
        {
            base.Setup();
            ResetCategoryAndDefinition();
            CreateBlueprintEntity();
        }

        public override void Teardown()
        {
            base.Teardown();
            RemoveBlueprintEntity();
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartBuild();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndBuild();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            if (isLocked) return;

            ResetBlueprintEntity();

            events.onBlueprintEntityUpdated?.Invoke(blueprintEntity);
        }

        public void SetSelectedEntityKey(Type entityType)
        {
            isLocked = true;
            Type previousEntityType = this.selectedEntityType;
            this.selectedEntityType = entityType;

            ResetCategoryAndDefinition();
            ResetBlueprintEntity();

            events.onSelectedEntityKeyUpdated?.Invoke(this.selectedEntityType, previousEntityType);
            isLocked = false;
        }

        public void SetSelectedEntityCategory(string entityCategory)
        {
            this.selectedEntityCategory = entityCategory;

            this.selectedEntityDefinition = Registry.Definitions.Entities.Queries.FindFirstInCategory(selectedEntityType, selectedEntityCategory);

            ResetBlueprintEntity();

            events.onSelectedEntityCategoryUpdated?.Invoke(entityCategory);
            events.onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }


        // TODO - this should probably use key instead of title string
        public void SetSelectedEntityDefinition(string keyLabel)
        {
            this.selectedEntityDefinition = Registry.Definitions.Entities.Queries.FindDefinitionByKeyLabel(selectedEntityDefinition, keyLabel);

            ResetBlueprintEntity();

            events.onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }

        /*
            Internals
        */
        void ResetCategoryAndDefinition()
        {
            selectedEntityCategory = Registry.Definitions.Entities.Queries.FindFirstCategory(selectedEntityType);
            selectedEntityDefinition = Registry.Definitions.Entities.Queries.FindFirstInCategory(selectedEntityType, selectedEntityCategory);
        }

        void StartBuild()
        {
            if (isLocked) return;
            buildIsActive = true;

            events.onBuildStart?.Invoke();
        }

        void EndBuild()
        {
            // EndBuild can be called when the mouse click up event happens outside the screen or over a UI element
            if (isLocked || !buildIsActive) return;
            buildIsActive = false;

            isLocked = true;

            blueprintEntity.validator.Validate(Registry.appState);

            if (blueprintEntity.validator.isValid)
            {
                BuildBlueprintEntity();
                CreateBlueprintEntity();
            }
            else
            {
                Registry.appState.Notifications.Add(
                    new ListWrapper<Notification>(
                        blueprintEntity.validator.errors.items
                            .Select(error => new Notification(error.message))
                            .ToList()
                    )
                );

                ResetBlueprintEntity();
            }

            events.onBuildEnd?.Invoke();

            isLocked = false;
        }

        void CreateBlueprintEntity()
        {
            blueprintEntity = Entity.CreateFromDefinition(selectedEntityDefinition);

            blueprintEntity.isInBlueprintMode = true;
            blueprintEntity.CalculateCellsFromSelectionBox(Registry.appState.UI.selectionBox);
            blueprintEntity.validator.Validate(Registry.appState);

            Registry.appState.Entities.Add(blueprintEntity);
        }

        void BuildBlueprintEntity()
        {
            Registry.appState.Entities.Build(blueprintEntity);

            blueprintEntity = null;
        }

        void RemoveBlueprintEntity()
        {
            if (blueprintEntity == null) return;

            Registry.appState.Entities.Remove(blueprintEntity);

            blueprintEntity = null;
        }

        void ResetBlueprintEntity()
        {
            RemoveBlueprintEntity();
            CreateBlueprintEntity();
        }
    }
}