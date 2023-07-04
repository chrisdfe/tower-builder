using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class EntityTypeButtonsRow : UIButtonsRowBase
    {
        public EntityTypeButtonsRow() : base() { }

        public override List<UISelectButton.Input> CreateButtonInputs() =>
            DataTypes.Entities.EntityTypeData.List
                .Select((entityTypeData) =>
                    new UISelectButton.Input()
                    {
                        label = entityTypeData.label,
                        value = entityTypeData.label
                    }
                )
                .ToList();

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            button.value == DataTypes.Entities.EntityTypeData.Get(Registry.appState.Tools.Build.Entities.selectedEntityType).label;

        public override void OnButtonClick(string value)
        {
            EntityTypeData entityTypeData = DataTypes.Entities.EntityTypeData.List.Find(entityTypeData => entityTypeData.label == value);
            Type newEntityType = entityTypeData.EntityType;
            Registry.appState.Tools.Build.Entities.SetSelectedEntityKey(newEntityType);
        }
    }
}