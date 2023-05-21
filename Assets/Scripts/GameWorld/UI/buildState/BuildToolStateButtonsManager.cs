using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
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

        Transform entitySelectButtonWrappers;
        Transform entityTypeButtonsWrapper;
        Transform categoryButtonsWrapper;
        Transform definitionButtonsWrapper;

        List<UISelectButton> entityTypeButtons = new List<UISelectButton>();
        List<UISelectButton> categoryButtons = new List<UISelectButton>();
        List<UISelectButton> definitionButtons = new List<UISelectButton>();

        const string entitySelectButtonWrappersName = "EntitySelectButtonWrappers";
        const string entityTypeButtonsWrapperName = "EntityTypeButtons";
        const string categoryButtonsWrapperName = "CategoryButtons";
        const string definitionButtonsWrapperName = "DefinitionButtons";

        void Awake()
        {
            entitySelectButtonWrappers = transform.Find(entitySelectButtonWrappersName);

            entityTypeButtonsWrapper = entitySelectButtonWrappers.Find(entityTypeButtonsWrapperName);
            categoryButtonsWrapper = entitySelectButtonWrappers.Find(categoryButtonsWrapperName);
            definitionButtonsWrapper = entitySelectButtonWrappers.Find(definitionButtonsWrapperName);

            Registry.appState.Tools.buildToolState.events.onSelectedEntityKeyUpdated += OnSelectedEntityKeyUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityCategoryUpdated += OnSelectedEntityCategoryUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;

            Setup();
        }

        public void Open()
        {
            gameObject.SetActive(true);

            HighlightSelectedEntityTypeButton();
            HighlightSelectedCategoryButton();
            HighlightSelectedDefinitionButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Setup()
        {
            ResetEntityTypeButtons();
            ResetCategoryButtons();
            ResetDefinitionButtons();
        }

        public void Teardown()
        {
            DestroyEntityTypeButtons();
            DestroyEntityTypeButtons();
            DestroyDefinitionButtons();
        }

        /* 
            Entity Type buttons
        */
        void ResetEntityTypeButtons()
        {
            DestroyEntityTypeButtons();
            CreateEntityTypeButtons();
        }

        void CreateEntityTypeButtons()
        {
            entityTypeButtons = UISelectButton.CreateButtonListFromInputList(entityTypeButtonsWrapper, GenerateEntityTypeButtonInputs());

            foreach (UISelectButton selectButton in entityTypeButtons)
            {
                selectButton.transform.SetParent(this.entityTypeButtonsWrapper, false);
                selectButton.onClick += OnEntityTypeButtonClick;
            }

            HighlightSelectedEntityTypeButton();
        }

        void HighlightSelectedEntityTypeButton()
        {
            foreach (UISelectButton button in entityTypeButtons)
            {
                button.SetSelected(button.value == Entity.TypeLabels.ValueFromKey(Registry.appState.Tools.buildToolState.selectedEntityType));
            }
        }

        void DestroyEntityTypeButtons()
        {
            foreach (UISelectButton entityTypeButton in entityTypeButtons)
            {
                entityTypeButton.onClick -= OnEntityTypeButtonClick;
                GameObject.Destroy(entityTypeButton.gameObject);
            }

            TransformUtils.DestroyChildren(entityTypeButtonsWrapper);
        }

        List<UISelectButton.Input> GenerateEntityTypeButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();
            Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            return Entity.TypeLabels.labels
                .Select(label =>
                    new UISelectButton.Input()
                    {
                        label = label,
                        value = label
                    }
                )
                .ToList();
        }

        /* 
            Entity Category buttons
        */
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
            foreach (UISelectButton categoryButton in categoryButtons)
            {
                categoryButton.onClick -= OnCategoryButtonClick;
                GameObject.Destroy(categoryButton.gameObject);
            }

            TransformUtils.DestroyChildren(categoryButtonsWrapper);
        }

        List<UISelectButton.Input> GenerateCategoryButtonInputs()
        {
            List<UISelectButton> result = new List<UISelectButton>();
            Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            var allEntityCategories = Registry.Definitions.Entities.Queries.FindAllCategories(selectedEntityType);

            return allEntityCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        /* 
            Entity Definition buttons
        */
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
            foreach (UISelectButton definitionButton in definitionButtons)
            {
                definitionButton.onClick -= OnDefinitionButtonClick;
                GameObject.Destroy(definitionButton.gameObject);
            }

            TransformUtils.DestroyChildren(definitionButtonsWrapper);
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
        void OnEntityTypeButtonClick(string newEntityTypeName)
        {
            Debug.Log("newEntityTypeName");
            Debug.Log(newEntityTypeName);
            Type newEntityType = Entity.TypeLabels.KeyFromValue(newEntityTypeName);
            Registry.appState.Tools.buildToolState.SetSelectedEntityKey(newEntityType);
        }

        void OnCategoryButtonClick(string entityCategory)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityCategory(entityCategory);
        }

        void OnDefinitionButtonClick(string title)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(title);
        }

        // EntityType buttons
        void OnSelectedEntityKeyUpdated(Type entityType, Type previousEntityType)
        {
            if (entityType == previousEntityType) return;

            HighlightSelectedEntityTypeButton();
            ResetCategoryButtons();
            ResetDefinitionButtons();
        }

        void OnSelectedEntityCategoryUpdated(string newEntityCategory)
        {
            // Build new set of definition buttons
            ResetCategoryButtons();
            ResetDefinitionButtons();
        }

        void OnSelectedEntityDefinitionUpdated(EntityDefinition entityDefinition)
        {
            // ResetDefinitionButtons();
            HighlightSelectedDefinitionButton();
        }
    }
}
