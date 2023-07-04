using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class EntityDefinitionButtonsRow : UIButtonsRowBase
    {
        public EntityDefinitionButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs()
        {
            Type selectedEntityType = Registry.appState.Tools.Build.Entities.selectedEntityType;
            string currentCategory = Registry.appState.Tools.Build.Entities.selectedEntityCategory;

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
            button.value == Registry.appState.Tools.Build.Entities.selectedEntityDefinition.key;

        public override void OnButtonClick(string value)
        {
            Registry.appState.Tools.Build.Entities.SetSelectedEntityDefinition(value);
        }
    }
}