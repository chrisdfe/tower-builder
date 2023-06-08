using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildTypeButtonsRow : BuildToolStateButtonsRowBase
    {
        public BuildTypeButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs() =>
            new List<UISelectButton.Input>() {
                new UISelectButton.Input() {
                    label = "Entities",
                    value = "Entities",
                },
                new UISelectButton.Input() {
                    label = "Rooms",
                    value = "Rooms",
                },
            };

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            false;
        // button.value == Registry.appState.Tools.buildToolState.selectedEntityCategory;

        public override void OnButtonClick(string value)
        {
            // Registry.appState.Tools.buildToolState.SetSelectedEntityCategory(value);
            Debug.Log("value: " + value);
        }
    }
}