
using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.Definitions.Templates;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
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
            // RoomTemplate selectedRoomTemplate = Registry.roomTemplates.FindByKey(roomTemplateKey);
            // Registry.appState.Tools.buildToolState.SetSelectedRoomTemplate(selectedRoomTemplate);
        }

        List<RoomTemplate> GetRoomTemplatesForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.roomEntityType.selectedRoomCategory;
            return Registry.roomTemplates.FindByCategory(currentCategory);
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