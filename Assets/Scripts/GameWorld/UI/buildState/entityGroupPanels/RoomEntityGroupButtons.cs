
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
    class RoomEntityGroupButtons : EntityGroupButtonsBase
    {
        public RoomEntityGroupButtons(Transform panelWrapper) : base(panelWrapper)
        {
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.events.onSelectedRoomCategoryUpdated += OnSelectedRoomCategoryUpdated;
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.events.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        protected override List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<string> allRoomCategories = Registry.roomTemplates.FindAllRoomCategories();
            return allRoomCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        protected override List<UISelectButton.Input> GenerateTemplateButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<RoomTemplate> currentRoomTemplates = GetRoomTemplatesForCurrentCategory();
            return currentRoomTemplates.Select(roomTemplate => new UISelectButton.Input() { label = roomTemplate.title, value = roomTemplate.key }).ToList();
        }

        protected override void OnCategoryButtonClick(string roomCategory)
        {
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.SetSelectedRoomCategory(roomCategory);
        }

        protected override void OnTemplateButtonClick(string roomTemplateKey)
        {
            RoomTemplate selectedRoomTemplate = Registry.roomTemplates.FindByKey(roomTemplateKey);
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.SetSelectedRoomTemplate(selectedRoomTemplate);
        }

        List<RoomTemplate> GetRoomTemplatesForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.roomEntityType.selectedRoomCategory;
            return Registry.roomTemplates.FindByCategory(currentCategory);
        }

        void OnSelectedRoomCategoryUpdated(string newRoomCategory)
        {
            SetSelectedCategory(newRoomCategory);
        }

        void OnSelectedRoomTemplateUpdated(RoomTemplate roomTemplate)
        {
            SetSelectedTemplate(roomTemplate.key);
        }
    }
}