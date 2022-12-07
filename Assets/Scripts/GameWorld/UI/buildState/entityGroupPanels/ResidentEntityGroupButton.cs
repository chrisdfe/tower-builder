
using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.Definitions;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    class ResidentEntityGroupButtons : EntityGroupButtonsBase
    {
        public ResidentEntityGroupButtons(Transform panelWrapper) : base(panelWrapper)
        {
        }

        protected override List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<string> allResidentCategories = new List<string> { "one", "two" };
            return allResidentCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        protected override List<UISelectButton.Input> GenerateTemplateButtonInputs()
        {
            List<string> residentTemplates = new List<string> { "one", "two" };
            return residentTemplates.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }


        protected override void OnCategoryButtonClick(string residentCategory)
        {
            // Registry.appState.Tools.buildToolState.SetSelectedRoomCategory(roomCategory);
        }

        protected override void OnTemplateButtonClick(string residentTemplateKey)
        {
            // RoomTemplate selectedRoomTemplate = Registry.roomDefinitions.FindByKey(roomTemplateKey);
            // Registry.appState.Tools.buildToolState.SetSelectedRoomTemplate(selectedRoomTemplate);
        }

        List<RoomTemplate> GetRoomDefinitionsForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.roomEntityType.selectedRoomCategory;
            return Registry.definitions.rooms.queries.FindByCategory(currentCategory);
        }

        // void OnSelectedRoomCategoryUpdated(string newRoomCategory)
        // {
        //     SetSelectedCategory(newRoomCategory);
        // }

        // void OnSelectedRoomTemplateUpdated(RoomTemplate roomTemplate)
        // {
        //     SetSelectedTemplate(roomTemplate.key);
        // }
    }
}