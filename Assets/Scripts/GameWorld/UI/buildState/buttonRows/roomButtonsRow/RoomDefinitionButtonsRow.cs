using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class RoomDefinitionButtonsRow : BuildToolStateButtonsRowBase
    {
        public RoomDefinitionButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs() =>
            DataTypes.EntityGroups.Definitions.Rooms.Definitions.items.Select(roomDefinition =>
                new UISelectButton.Input()
                {
                    label = roomDefinition.title,
                    value = roomDefinition.key
                }
            ).ToList();

        public override void Setup()
        {
            base.Setup();

            Registry.appState.Tools.Build.Rooms.onRoomKeyUpdated += OnRoomKeyUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            Registry.appState.Tools.Build.Rooms.onRoomKeyUpdated -= OnRoomKeyUpdated;
        }

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == Registry.appState.Tools.Build.Rooms.selectedRoomKey;

        public override void OnButtonClick(string value)
        {
            Registry.appState.Tools.Build.Rooms.SetSelectedRoomKey(value);
        }

        void OnRoomKeyUpdated(string value)
        {
            HighlightSelectedButton();
        }
    }
}