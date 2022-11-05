
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
        protected override string categoryButtonsWrapperName { get { return "CategoryButtons"; } }
        protected override string templateButtonsWrapperName { get { return "TemplateButtons"; } }

        public RoomEntityGroupButtons(Transform panelWrapper) : base(panelWrapper)
        {
            Registry.appState.Tools.buildToolSubState.events.onSelectedRoomCategoryUpdated += OnSelectedRoomCategoryUpdated;
            Registry.appState.Tools.buildToolSubState.events.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        protected override List<UISelectButton> GenerateCategoryButtons()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<string> allRoomCategories = Registry.roomTemplates.FindAllRoomCategories();
            string currentCategory = Registry.appState.Tools.buildToolSubState.selectedRoomCategory;

            foreach (string category in allRoomCategories)
            {
                UISelectButton categoryButton = UISelectButton.Create(new UISelectButton.Input() { label = category, value = category });
                result.Add(categoryButton);
            }

            return result;
        }

        protected override List<UISelectButton> GenerateTemplateButtons()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<RoomTemplate> currentRoomTemplates = GetRoomTemplatesForCurrentCategory();
            RoomTemplate currentTemplate = Registry.appState.Tools.buildToolSubState.selectedRoomTemplate;

            foreach (RoomTemplate roomTemplate in currentRoomTemplates)
            {
                UISelectButton selectButton = UISelectButton.Create(new UISelectButton.Input() { label = roomTemplate.title, value = roomTemplate.key });

                result.Add(selectButton);
            }

            return result;
        }

        protected override void OnCategoryButtonClick(string roomCategory)
        {
            Registry.appState.Tools.buildToolSubState.SetSelectedRoomCategory(roomCategory);
        }

        protected override void OnTemplateButtonClick(string roomTemplateKey)
        {
            RoomTemplate selectedRoomTemplate = Registry.roomTemplates.FindByKey(roomTemplateKey);
            Registry.appState.Tools.buildToolSubState.SetSelectedRoomTemplate(selectedRoomTemplate);
        }

        List<RoomTemplate> GetRoomTemplatesForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolSubState.selectedRoomCategory;
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