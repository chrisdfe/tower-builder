
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
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.events.onSelectedRoomDefinitionUpdated += OnSelectedRoomDefinitionUpdated;
        }

        protected override List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<string> allRoomCategories = Registry.Definitions.Entities.Rooms.Queries.FindAllCategories();
            return allRoomCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        protected override List<UISelectButton.Input> GenerateDefinitionButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();

            List<RoomDefinition> currentRoomDefinitions = GetRoomDefinitionsForCurrentCategory();
            return currentRoomDefinitions.Select((roomDefinition) =>
                new UISelectButton.Input()
                {
                    label = roomDefinition.title,
                    value = Room.KeyLabelMap.ValueFromKey(roomDefinition.key)
                }).ToList();
        }

        protected override void OnCategoryButtonClick(string roomCategory)
        {
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.SetSelectedRoomCategory(roomCategory);
        }

        protected override void OnDefinitionButtonClick(string roomDefinitionLabel)
        {
            RoomDefinition selectedRoomDefinition = Registry.Definitions.Entities.Rooms.Queries.FindByKey(Room.KeyLabelMap.KeyFromValue(roomDefinitionLabel));
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.SetSelectedRoomDefinition(selectedRoomDefinition);
        }

        List<RoomDefinition> GetRoomDefinitionsForCurrentCategory()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.subStates.roomEntityType.selectedRoomCategory;
            return Registry.Definitions.Entities.Rooms.Queries.FindByCategory(currentCategory);
        }

        void OnSelectedRoomCategoryUpdated(string newRoomCategory)
        {
            SetSelectedCategory(newRoomCategory);
        }

        void OnSelectedRoomDefinitionUpdated(RoomDefinition roomDefinition)
        {
            SetSelectedDefinition(Room.KeyLabelMap.ValueFromKey(roomDefinition.key));
        }
    }
}