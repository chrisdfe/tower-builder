using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;

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

        public delegate void blueprintUpdateEvent(Entity blueprintEntity);
        public blueprintUpdateEvent onBlueprintEntityUpdated;

        /*
            State
        */
        public Type selectedEntityType { get; private set; } = typeof(DataTypes.Entities.Foundations.Foundation);
        public string selectedEntityCategory { get; private set; } = "";
        public EntityDefinition selectedEntityDefinition { get; private set; } = null;

        public Entity blueprintEntity { get; private set; } = null;

        public State(AppState appState, Tools.State toolState, Build.State buildState, Input input) : base(appState, toolState, buildState)
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

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            if (isLocked) return;

            ResetBlueprintEntity();

            onBlueprintEntityUpdated?.Invoke(blueprintEntity);
        }

        /*
            Public API
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

        public override void OnBuildStart() { }

        public override void OnBuildEnd()
        {
            blueprintEntity.Validate(Registry.appState);

            if (blueprintEntity.isValid)
            {
                BuildBlueprintEntity();
                CreateBlueprintEntity();
            }
            else
            {
                Registry.appState.Notifications.Add(
                    new ListWrapper<Notification>(
                        blueprintEntity.validationErrors.items
                            .Select(error => new Notification(error.message))
                            .ToList()
                    )
                );

                ResetBlueprintEntity();
            }
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
            blueprintEntity.Validate(Registry.appState);

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