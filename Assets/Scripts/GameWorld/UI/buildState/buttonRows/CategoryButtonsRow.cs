using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class CategoryButtonsRow : BuildToolStateButtonsRowBase
    {
        public CategoryButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs()
        {
            Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            List<string> allEntityCategories = DataTypes.Entities.Definitions.FindAllCategories(selectedEntityType);

            return allEntityCategories
                .Select(category =>
                    new UISelectButton.Input()
                    {
                        label = category,
                        value = category
                    }
                ).ToList();
        }

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == Registry.appState.Tools.buildToolState.selectedEntityCategory;

        public override void OnButtonClick(string value)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityCategory(value);
        }
    }
}