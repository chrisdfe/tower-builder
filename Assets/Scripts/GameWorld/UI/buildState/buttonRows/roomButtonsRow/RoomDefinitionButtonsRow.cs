using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class RoomDefinitionButtonsRow : BuildToolStateButtonsRowBase
    {
        public RoomDefinitionButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs() =>
            new List<UISelectButton.Input>() {
                new UISelectButton.Input() {
                    label = "hello",
                    value = "hello"
                },
                new UISelectButton.Input() {
                    label = "another one",
                    value = "another one"
                }
            };

        public override void Setup()
        {
            base.Setup();

            Registry.appState.Tools.Build.Rooms.events.onRoomKeyUpdated += OnRoomKeyUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            Registry.appState.Tools.Build.Rooms.events.onRoomKeyUpdated -= OnRoomKeyUpdated;
        }

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == Registry.appState.Tools.Build.Rooms.selectedRoomKey;

        public override void OnButtonClick(string value)
        {
            // Type newEntityType = DataTypes.Entities.Constants.TypeLabels.KeyFromValue(value);
            Debug.Log("value: " + value);
            Registry.appState.Tools.Build.Rooms.SetSelectedRoomKey(value);
        }

        void OnRoomKeyUpdated(string value)
        {
            HighlightSelectedButton();
        }
    }
}