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
            DataTypes.Entities.Constants.TypeLabels.values
                .Select(label =>
                    new UISelectButton.Input()
                    {
                        label = label,
                        value = label
                    }
                )
                .ToList();

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == DataTypes.Entities.Constants.TypeLabels.ValueFromKey(Registry.appState.Tools.Build.Entities.selectedEntityType);

        public override void OnButtonClick(string value)
        {
            Type newEntityType = DataTypes.Entities.Constants.TypeLabels.KeyFromValue(value);
            Registry.appState.Tools.Build.Entities.SetSelectedEntityKey(newEntityType);
        }
    }
}