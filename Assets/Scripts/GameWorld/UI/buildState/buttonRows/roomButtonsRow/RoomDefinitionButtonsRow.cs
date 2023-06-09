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
                }
            };

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == DataTypes.Entities.Constants.TypeLabels.ValueFromKey(Registry.appState.Tools.Build.Entities.selectedEntityType);

        public override void OnButtonClick(string value)
        {
            // Type newEntityType = DataTypes.Entities.Constants.TypeLabels.KeyFromValue(value);
            Debug.Log("value: " + value);
            Registry.appState.Tools.Build.Rooms.SetSelectedRoomKey(value);
        }
    }
}