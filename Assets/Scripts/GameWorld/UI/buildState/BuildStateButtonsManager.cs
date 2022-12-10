using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildStateButtonsManager : MonoBehaviour
    {
        Color originalColor;
        Button currentButton;

        Transform entityTypeButtonsWrapper;
        Transform entityTypeBuildButtonsWrapper;

        EntityTypeBuildButtons entityTypeBuildButtons;

        List<UISelectButton> entityTypeSelectButtons;

        void Awake()
        {
            entityTypeButtonsWrapper = transform.Find("EntityTypeButtonsWrapper");
            entityTypeBuildButtonsWrapper = transform.Find("EntityTypeBuildButtonsWrapper");

            entityTypeSelectButtons = Entity.TypeLabels.labels.Select(label =>
            {
                UISelectButton entityButton = UISelectButton.Create(
                    entityTypeButtonsWrapper,
                    new UISelectButton.Input()
                    {
                        label = label,
                        value = label
                    });
                entityButton.onClick += OnEntityGroupButtonClick;
                return entityButton;
            }).ToList();


            entityTypeBuildButtons = new EntityTypeBuildButtons(entityTypeBuildButtonsWrapper);
            entityTypeBuildButtons.Setup();

            Registry.appState.Tools.buildToolState.events.onSelectedEntityKeyUpdated += OnSelectedEntityKeyUpdated;
        }

        /*
            Event Handlers
        */
        void OnEntityGroupButtonClick(string newCategory)
        {
            Entity.Type newEntityType = Entity.TypeLabels.KeyFromValue(newCategory);
            Registry.appState.Tools.buildToolState.SetSelectedEntityKey(newEntityType);
        }

        void OnSelectedEntityKeyUpdated(Entity.Type entityType, Entity.Type previousEntityType)
        {
            if (entityType == previousEntityType) return;

            entityTypeSelectButtons.ForEach((selectButton) =>
            {
                bool isSelected = Entity.TypeLabels.KeyFromValue(selectButton.value) == entityType;
                selectButton.SetSelected(isSelected);
            });

            if (entityTypeBuildButtons != null)
            {
                entityTypeBuildButtons.Teardown();
                entityTypeBuildButtons = null;
                entityTypeBuildButtons = new EntityTypeBuildButtons(entityTypeBuildButtonsWrapper);
            }
        }
    }
}
