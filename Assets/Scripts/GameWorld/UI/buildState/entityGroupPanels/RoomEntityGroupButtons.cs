
using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities.Rooms;
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

            List<string> allRoomCategories = Registry.definitions.rooms.queries.FindAllCategories();
            return allRoomCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        protected override List<UISelectButton.Input> GenerateTemplateButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<RoomTemplate> currentRoomDefinitions = GetRoomDefinitionsForCurrentCategory();
            return currentRoomDefinitions.Select((roomTemplate) =>
                new UISelectButton.Input()
                {
                    label = roomTemplate.title,
                    value = Room.KeyLabelMap.ValueFromKey(roomTemplate.key)
                }).ToList();
        }

        protected override void OnCategoryButtonClick(string roomCategory)
        {
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.SetSelectedRoomCategory(roomCategory);
        }

        protected override void OnTemplateButtonClick(string roomTemplateLabel)
        {
            RoomTemplate selectedRoomTemplate = Registry.definitions.rooms.queries.FindByKey(Room.KeyLabelMap.KeyFromValue(roomTemplateLabel));
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.SetSelectedRoomTemplate(selectedRoomTemplate);
        }

        List<RoomTemplate> GetRoomDefinitionsForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.roomEntityType.selectedRoomCategory;
            return Registry.definitions.rooms.queries.FindByCategory(currentCategory);
        }

        void OnSelectedRoomCategoryUpdated(string newRoomCategory)
        {
            SetSelectedCategory(newRoomCategory);
        }

        void OnSelectedRoomTemplateUpdated(RoomTemplate roomTemplate)
        {
            SetSelectedTemplate(Room.KeyLabelMap.ValueFromKey(roomTemplate.key));
        }
    }
}