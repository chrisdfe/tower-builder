using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildToolStateButtonsManager : MonoBehaviour
    {
        Color originalColor;
        Button currentButton;

        Transform entityTypeButtonsWrapper;
        Transform entityTypeBuildButtonsWrapper;
        Transform categoryButtonsWrapper;
        Transform definitionButtonsWrapper;

        List<UISelectButton> entityTypeSelectButtons = new List<UISelectButton>();
        List<UISelectButton> categoryButtons = new List<UISelectButton>();
        List<UISelectButton> definitionButtons = new List<UISelectButton>();

        const string entityTypeButtonsWrapperName = "EntityTypeButtonsWrapper";
        const string entityTypeBuildButtonsWrapperName = "EntityTypeBuildButtonsWrapper";
        const string categoryButtonsWrapperName = "CategoryButtons";
        const string definitionButtonsWrapperName = "DefinitionButtons";

        void Awake()
        {
            entityTypeButtonsWrapper = transform.Find(entityTypeButtonsWrapperName);
            entityTypeBuildButtonsWrapper = transform.Find(entityTypeBuildButtonsWrapperName);

            entityTypeSelectButtons = Entity.TypeLabels.labels
                .Select(label =>
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
                })
                .ToList();

            categoryButtonsWrapper = entityTypeBuildButtonsWrapper.Find(categoryButtonsWrapperName);
            definitionButtonsWrapper = entityTypeBuildButtonsWrapper.Find(definitionButtonsWrapperName);

            Registry.appState.Tools.buildToolState.events.onSelectedEntityKeyUpdated += OnSelectedEntityKeyUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityCategoryUpdated += OnSelectedEntityCategoryUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;

            Setup();
        }

        public void Setup()
        {
            ResetCategoryButtons();
            ResetDefinitionButtons();
        }

        public void Teardown()
        {
            DestroyCategoryButtons();
            DestroyDefinitionButtons();
        }

        void ResetCategoryButtons()
        {
            DestroyCategoryButtons();
            CreateCategoryButtons();
        }

        void CreateCategoryButtons()
        {
            categoryButtons = UISelectButton.CreateButtonListFromInputList(categoryButtonsWrapper, GenerateCategoryButtonInputs());

            foreach (UISelectButton selectButton in categoryButtons)
            {
                selectButton.transform.SetParent(this.categoryButtonsWrapper, false);
                selectButton.onClick += OnCategoryButtonClick;
            }

            HighlightSelectedCategoryButton();
        }

        void HighlightSelectedCategoryButton()
        {
            foreach (UISelectButton button in categoryButtons)
            {
                button.SetSelected(button.value == Registry.appState.Tools.buildToolState.selectedEntityCategory);
            }
        }

        void DestroyCategoryButtons()
        {
            TransformUtils.DestroyChildren(categoryButtonsWrapper);

            foreach (UISelectButton categoryButton in categoryButtons)
            {
                categoryButton.onClick -= OnCategoryButtonClick;
                GameObject.Destroy(categoryButton.gameObject);
            }
        }

        void ResetDefinitionButtons()
        {
            DestroyDefinitionButtons();
            CreateDefinitionButtons();
        }

        void CreateDefinitionButtons()
        {
            definitionButtons = UISelectButton.CreateButtonListFromInputList(definitionButtonsWrapper, GenerateDefinitionButtonInputs());

            foreach (UISelectButton definitionButton in definitionButtons)
            {
                definitionButton.onClick += OnDefinitionButtonClick;
            }

            HighlightSelectedDefinitionButton();
        }

        void HighlightSelectedDefinitionButton()
        {
            foreach (UISelectButton button in definitionButtons)
            {
                button.SetSelected(button.value == Entity.GetEntityDefinitionLabel(Registry.appState.Tools.buildToolState.selectedEntityDefinition));
            }
        }

        void DestroyDefinitionButtons()
        {
            TransformUtils.DestroyChildren(definitionButtonsWrapper);
            foreach (UISelectButton definitionButton in definitionButtons)
            {
                definitionButton.onClick -= OnDefinitionButtonClick;
                GameObject.Destroy(definitionButton.gameObject);
            }
        }

        List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();
            Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            var allEntityCategories = Registry.Definitions.Entities.Queries.FindAllCategories(selectedEntityType);

            return allEntityCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        List<UISelectButton.Input> GenerateDefinitionButtonInputs()
        {
            Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;
            string currentCategory = Registry.appState.Tools.buildToolState.selectedEntityCategory;

            return Registry.Definitions.Entities.Queries.FindByCategory(selectedEntityType, currentCategory)
                .items.Select((definition) =>
                    new UISelectButton.Input()
                    {
                        label = definition.title,
                        value = Entity.GetEntityDefinitionLabel(definition)
                    }
                ).ToList();
        }

        /*
            Event Handlers
        */
        void OnCategoryButtonClick(string entityCategory)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityCategory(entityCategory);
        }

        void OnDefinitionButtonClick(string title)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(title);
        }

        void OnSelectedEntityCategoryUpdated(string newEntityCategory)
        {
            // Build new set of definition buttons
            ResetCategoryButtons();
            ResetDefinitionButtons();
        }

        void OnSelectedEntityDefinitionUpdated(EntityDefinition entityDefinition)
        {
            // Entity.GetEntityDefinitionLabel(entityDefinition)

            // ResetDefinitionButtons();
            HighlightSelectedDefinitionButton();

            // Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(label);
        }

        void OnEntityGroupButtonClick(string newCategory)
        {
            Type newEntityType = Entity.TypeLabels.KeyFromValue(newCategory);
            Registry.appState.Tools.buildToolState.SetSelectedEntityKey(newEntityType);
        }

        void OnSelectedEntityKeyUpdated(Type entityType, Type previousEntityType)
        {
            if (entityType == previousEntityType) return;

            entityTypeSelectButtons.ForEach((selectButton) =>
            {
                bool isSelected = Entity.TypeLabels.KeyFromValue(selectButton.value) == entityType;
                selectButton.SetSelected(isSelected);
            });

            ResetCategoryButtons();
            ResetDefinitionButtons();
        }
    }
}
