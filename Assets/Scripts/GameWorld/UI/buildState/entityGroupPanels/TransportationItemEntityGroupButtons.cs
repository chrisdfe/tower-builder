
using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.Definitions;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    class TransportationItemEntityGroupButtons : EntityGroupButtonsBase
    {
        public TransportationItemEntityGroupButtons(Transform panelWrapper) : base(panelWrapper) { }

        public override void Setup()
        {
            base.Setup();

            Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.events.onSelectedCategoryUpdated += OnSelectedCategoryUpdated;
            Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.events.onSelectedTemplateUpdated += OnSelectedTemplateUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.events.onSelectedCategoryUpdated -= OnSelectedCategoryUpdated;
            Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.events.onSelectedTemplateUpdated -= OnSelectedTemplateUpdated;
        }

        protected override List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<string> allCategories = Registry.definitions.transportationItems.queries.FindAllCategories();
            return allCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        protected override List<UISelectButton.Input> GenerateTemplateButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<TransportationItemTemplate> transportationItemDefinitions = GetDefinitionsForCurrentCategory();
            return transportationItemDefinitions.Select(template =>
            {
                string label = TransportationItem.KeyLabelMap.ValueFromKey(template.key);
                return new UISelectButton.Input() { label = template.title, value = label };
            }).ToList();
        }

        protected override void OnCategoryButtonClick(string category)
        {
            Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.SetSelectedCategory(category);
        }

        protected override void OnTemplateButtonClick(string templateLabel)
        {
            TransportationItem.Key key = TransportationItem.KeyLabelMap.KeyFromValue(templateLabel);
            TransportationItemTemplate selectedTemplate = Registry.definitions.transportationItems.queries.FindByKey(key);
            Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.SetSelectedTemplate(selectedTemplate);
        }

        List<TransportationItemTemplate> GetDefinitionsForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.transportationItemEntityType.selectedCategory;
            return Registry.definitions.transportationItems.queries.FindByCategory(currentCategory);
        }

        /*
            Event Handlers
        */
        void OnSelectedCategoryUpdated(string newCategory)
        {
            SetSelectedCategory(newCategory);
        }

        void OnSelectedTemplateUpdated(TransportationItemTemplate template)
        {
            string label = TransportationItem.KeyLabelMap.ValueFromKey(template.key);
            SetSelectedTemplate(label);
        }
    }
}