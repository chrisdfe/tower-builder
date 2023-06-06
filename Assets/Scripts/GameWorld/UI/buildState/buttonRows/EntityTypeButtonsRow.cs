using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class EntityTypeButtonsRow : BuildToolStateButtonsRowBase
    {
        public EntityTypeButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs() =>
            Entity.TypeLabels.labels
                .Select(label =>
                    new UISelectButton.Input()
                    {
                        label = label,
                        value = label
                    }
                )
                .ToList();

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == Entity.TypeLabels.ValueFromKey(Registry.appState.Tools.buildToolState.selectedEntityType);

        public override void OnButtonClick(string value)
        {
            Type newEntityType = Entity.TypeLabels.KeyFromValue(value);
            Registry.appState.Tools.buildToolState.SetSelectedEntityKey(newEntityType);
        }
    }
}