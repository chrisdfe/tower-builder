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

            return Registry.Definitions.Entities.Queries.FindByCategory(selectedEntityType, currentCategory)
                .items.Select((definition) =>
                    new UISelectButton.Input()
                    {
                        label = definition.title,
                        value = DataTypes.Entities.Constants.GetEntityDefinitionLabel(definition)
                    }
                ).ToList();
        }

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == DataTypes.Entities.Constants.GetEntityDefinitionLabel(Registry.appState.Tools.buildToolState.selectedEntityDefinition);

        public override void OnButtonClick(string value)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(value);
        }
    }
}