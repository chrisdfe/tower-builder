using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class DefinitionButtonsRow : BuildToolStateButtonsRowBase
    {
        public DefinitionButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs()
        {
            Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;
            string currentCategory = Registry.appState.Tools.buildToolState.selectedEntityCategory;

            return DataTypes.Entities.Definitions.FindByCategory(selectedEntityType, currentCategory)
                .items.Select((definition) =>
                    new UISelectButton.Input()
                    {
                        label = definition.title,
                        value = definition.key
                    }
                ).ToList();
        }

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == Registry.appState.Tools.buildToolState.selectedEntityDefinition.key;

        public override void OnButtonClick(string value)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(value);
        }
    }
}