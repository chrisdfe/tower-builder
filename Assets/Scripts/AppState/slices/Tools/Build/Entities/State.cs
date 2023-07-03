using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Build.Entities
{
    public class State : BuildModeStateBase
    {
        public struct Input
        {
            public Type selectedEntityType;
            public string selectedEntityCategory;
        }

        /*
            Events
        */
        public delegate void SelectedEntityKeyEvent(Type entityKey, Type previousEntityType);
        public SelectedEntityKeyEvent onSelectedEntityKeyUpdated;

        public delegate void SelectedEntityCategoryEvent(string selectedEntityCategory);
        public SelectedEntityCategoryEvent onSelectedEntityCategoryUpdated;

        public delegate void SelectedEntityDefinitionEvent(EntityDefinition selectedEntityDefinition);
        public SelectedEntityDefinitionEvent onSelectedEntityDefinitionUpdated;

        public delegate void BlueprintEvent(Entity blueprintEntity);
        public BlueprintEvent onBlueprintEntityUpdated;
        public BlueprintEvent onBlueprintEntityPositionUpdated;

        /*
            State
        */
        public Type selectedEntityType { get; private set; } = DEFAULT_TYPE;
        public string selectedEntityCategory { get; private set; } = "";
        public EntityDefinition selectedEntityDefinition { get; private set; } = null;

        public Entity blueprintEntity { get; private set; } = null;

        static Type DEFAULT_TYPE = typeof(DataTypes.Entities.Foundations.Foundation);

        public State(AppState appState, Input input) : base(appState)
        {
            ResetCategoryAndDefinition();
        }

        /*
            Lifecycle
        */
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

        /*
            Public Interface
        */
        public void SetSelectedEntityKey(Type entityType)
        {
            isLocked = true;
            Type previousEntityType = this.selectedEntityType;
            this.selectedEntityType = entityType;

            ResetCategoryAndDefinition();
            ResetBlueprintEntity();

            onSelectedEntityKeyUpdated?.Invoke(this.selectedEntityType, previousEntityType);
            isLocked = false;
        }

        public void SetSelectedEntityCategory(string entityCategory)
        {
            this.selectedEntityCategory = entityCategory;

            this.selectedEntityDefinition = DataTypes.Entities.Definitions.FindFirstInCategory(selectedEntityType, selectedEntityCategory);

            ResetBlueprintEntity();

            onSelectedEntityCategoryUpdated?.Invoke(entityCategory);
            onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }

        public void SetSelectedEntityDefinition(string keyLabel)
        {
            this.selectedEntityDefinition = DataTypes.Entities.Definitions.FindByKey(this.selectedEntityType, keyLabel);

            ResetBlueprintEntity();

            onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            base.OnSelectionBoxUpdated(selectionBox);

            if (isLocked) return;

            if (appState.Tools.Build.buildIsActive)
            {
                ResetBlueprintEntity();
            }
            else
            {
                appState.Entities.UpdateEntityOffsetCoordinates(blueprintEntity, selectionBox.cellCoordinatesList.bottomLeftCoordinates);
                blueprintEntity.ValidateBuild(appState);
                onBlueprintEntityUpdated?.Invoke(blueprintEntity);
                onBlueprintEntityPositionUpdated?.Invoke(blueprintEntity);
            }
        }

        public override void OnBuildEnd()
        {
            base.OnBuildEnd();

            blueprintEntity.ValidateBuild(Registry.appState);

            if (blueprintEntity.canBuild)
            {
                BuildBlueprintEntity();
                CreateBlueprintEntity();
            }
            else
            {
                Registry.appState.Notifications.Add(blueprintEntity.buildValidationErrors);

                ResetBlueprintEntity();
            }
        }

        public override void OnSelectionBoxReset(SelectionBox selectionBox)
        {
            ResetBlueprintEntity();
        }

        /*
            Internals
        */
        void ResetCategoryAndDefinition()
        {
            selectedEntityCategory = DataTypes.Entities.Definitions.FindFirstCategory(selectedEntityType);
            selectedEntityDefinition = DataTypes.Entities.Definitions.FindFirstInCategory(selectedEntityType, selectedEntityCategory);
        }

        void CreateBlueprintEntity()
        {
            blueprintEntity = Entity.CreateFromDefinition(selectedEntityDefinition);

            blueprintEntity.isInBlueprintMode = true;
            blueprintEntity.CalculateCellsFromSelectionBox(Registry.appState.UI.selectionBox);
            blueprintEntity.offsetCoordinates = Registry.appState.UI.selectionBox.cellCoordinatesList.bottomLeftCoordinates;
            blueprintEntity.ValidateBuild(Registry.appState);

            Registry.appState.Entities.Add(blueprintEntity);
            onBlueprintEntityUpdated?.Invoke(blueprintEntity);
        }

        void BuildBlueprintEntity()
        {
            Registry.appState.Entities.Build(blueprintEntity);
            blueprintEntity = null;
        }

        void RemoveBlueprintEntity()
        {
            if (blueprintEntity != null)
            {
                Registry.appState.Entities.Remove(blueprintEntity);
                blueprintEntity = null;
            }
        }

        void ResetBlueprintEntity()
        {
            RemoveBlueprintEntity();
            CreateBlueprintEntity();
        }
    }
}