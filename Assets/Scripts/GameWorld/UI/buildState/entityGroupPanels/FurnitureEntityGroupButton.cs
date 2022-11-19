
using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.Definitions;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    class FurnitureEntityGroupButtons : EntityGroupButtonsBase
    {
        public FurnitureEntityGroupButtons(Transform panelWrapper) : base(panelWrapper)
        {
        }

        public override void Setup()
        {
            base.Setup();

            Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.events.onSelectedFurnitureCategoryUpdated += OnSelectedFurnitureCategoryUpdated;
            Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.events.onSelectedFurnitureTemplateUpdated += OnSelectedFurnitureTemplateUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.events.onSelectedFurnitureCategoryUpdated -= OnSelectedFurnitureCategoryUpdated;
            Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.events.onSelectedFurnitureTemplateUpdated -= OnSelectedFurnitureTemplateUpdated;
        }

        protected override List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<string> allCategories = Registry.definitions.furnitures.queries.FindAllCategories();
            return allCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        protected override List<UISelectButton.Input> GenerateTemplateButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<FurnitureTemplate> currentFurnitureDefinitions = GetFurnitureDefinitionsForCurrentCategory();
            return currentFurnitureDefinitions.Select(furnitureTemplate => new UISelectButton.Input() { label = furnitureTemplate.title, value = furnitureTemplate.key }).ToList();
        }

        protected override void OnCategoryButtonClick(string furnitureCategory)
        {
            Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.SetSelectedFurnitureCategory(furnitureCategory);
        }

        protected override void OnTemplateButtonClick(string furnitureTemplateKey)
        {
            FurnitureTemplate selectedFurnitureTemplate = Registry.definitions.furnitures.queries.FindByKey(furnitureTemplateKey);
            Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.SetSelectedFurnitureTemplate(selectedFurnitureTemplate);
        }

        List<FurnitureTemplate> GetFurnitureDefinitionsForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.furnitureEntityType.selectedFurnitureCategory;
            return Registry.definitions.furnitures.queries.FindByCategory(currentCategory);
        }

        void OnSelectedFurnitureCategoryUpdated(string newFurnitureCategory)
        {
            SetSelectedCategory(newFurnitureCategory);
        }

        void OnSelectedFurnitureTemplateUpdated(FurnitureTemplate furnitureTemplate)
        {
            SetSelectedTemplate(furnitureTemplate.key);
        }
    }
}