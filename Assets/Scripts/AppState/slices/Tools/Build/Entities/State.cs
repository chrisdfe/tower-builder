using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
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

        /*
            State
        */
        public Type selectedEntityType { get; private set; } = DEFAULT_TYPE;
        public string selectedEntityCategory { get; private set; } = "";
        public EntityDefinition selectedEntityDefinition { get; private set; } = null;

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
        }

        /*
            Public Interface
        */
        public override EntityGroup CalculateBlueprintEntityGroup()
        {
            EntityGroup blueprint = new EntityGroup();

            Entity blueprintEntity = Entity.CreateFromDefinition(selectedEntityDefinition);
            blueprintEntity.CalculateCellsFromSelectionBox(Registry.appState.UI.selectionBox.asRelativeSelectionBox);
            blueprint.Add(blueprintEntity);

            return blueprint;
        }

        public void SetSelectedEntityKey(Type entityType)
        {
            isLocked = true;
            Type previousEntityType = this.selectedEntityType;
            this.selectedEntityType = entityType;

            ResetCategoryAndDefinition();

            onResetRequested?.Invoke();
            onSelectedEntityKeyUpdated?.Invoke(this.selectedEntityType, previousEntityType);
            isLocked = false;
        }

        public void SetSelectedEntityCategory(string entityCategory)
        {
            this.selectedEntityCategory = entityCategory;

            this.selectedEntityDefinition = DataTypes.Entities.Definitions.FindFirstInCategory(selectedEntityType, selectedEntityCategory);

            onResetRequested?.Invoke();
            onSelectedEntityCategoryUpdated?.Invoke(entityCategory);
            onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }

        public void SetSelectedEntityDefinition(string keyLabel)
        {
            this.selectedEntityDefinition = DataTypes.Entities.Definitions.FindByKey(this.selectedEntityType, keyLabel);

            onResetRequested?.Invoke();
            onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }

        /*
            Internals
        */
        void ResetCategoryAndDefinition()
        {
            selectedEntityCategory = DataTypes.Entities.Definitions.FindFirstCategory(selectedEntityType);
            selectedEntityDefinition = DataTypes.Entities.Definitions.FindFirstInCategory(selectedEntityType, selectedEntityCategory);
        }
    }
}