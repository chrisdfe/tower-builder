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

namespace TowerBuilder.GameWorld.UI
{
    public class EntityTypeBuildButtons
    {
        Transform panelWrapper;
        Transform categoryButtonsWrapper;
        Transform definitionButtonsWrapper;

        List<UISelectButton> categoryButtons = new List<UISelectButton>();
        List<UISelectButton> definitionButtons = new List<UISelectButton>();

        const string categoryButtonsWrapperName = "CategoryButtons";
        const string definitionButtonsWrapperName = "DefinitionButtons";

        public EntityTypeBuildButtons(Transform panelWrapper)
        {
            this.panelWrapper = panelWrapper;

            categoryButtonsWrapper = panelWrapper.Find(categoryButtonsWrapperName);
            definitionButtonsWrapper = panelWrapper.Find(definitionButtonsWrapperName);

            Debug.Log("Registry.appState.Tools.buildToolState.selectedEntityType");
            Debug.Log(Registry.appState.Tools.buildToolState.selectedEntityType);

            Registry.appState.Tools.buildToolState.events.onSelectedEntityCategoryUpdated += OnSelectedEntityCategoryUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        public void Setup()
        {
            DestroyCategoryButtons();
            CreateCategoryButtons();
            HighlightSelectedCategoryButton();

            DestroyDefinitionButtons();
            CreateDefinitionButtons();
            HighlightSelectedDefinitionButton();
        }

        public void Teardown()
        {
            DestroyCategoryButtons();
            DestroyDefinitionButtons();
        }

        void CreateCategoryButtons()
        {
            categoryButtons = UISelectButton.CreateButtonListFromInputList(categoryButtonsWrapper, GenerateCategoryButtonInputs());

            foreach (UISelectButton selectButton in categoryButtons)
            {
                selectButton.transform.SetParent(this.categoryButtonsWrapper, false);
                selectButton.onClick += OnCategoryButtonClick;
            }
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

        void CreateDefinitionButtons()
        {
            definitionButtons = UISelectButton.CreateButtonListFromInputList(definitionButtonsWrapper, GenerateDefinitionButtonInputs());

            foreach (UISelectButton definitionButton in definitionButtons)
            {
                definitionButton.onClick += OnDefinitionButtonClick;
            }
        }

        void HighlightSelectedDefinitionButton()
        {
            foreach (UISelectButton button in definitionButtons)
            {
                button.SetSelected(button.value == Registry.appState.Tools.buildToolState.selectedEntityDefinition?.title);
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
            Entity.Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            List<string> allEntityCategories = selectedEntityType switch
            {
                Entity.Type.Room => Registry.Definitions.Entities.Rooms.Queries.FindAllCategories(),
                Entity.Type.Furniture => Registry.Definitions.Entities.Furnitures.Queries.FindAllCategories(),
                Entity.Type.Resident => Registry.Definitions.Entities.Residents.Queries.FindAllCategories(),
                Entity.Type.TransportationItem => Registry.Definitions.Entities.TransportationItems.Queries.FindAllCategories(),
                _ => null
            };

            return allEntityCategories.Select(category => new UISelectButton.Input() { label = category, value = category }).ToList();
        }

        List<UISelectButton.Input> GenerateDefinitionButtonInputs()
        {
            string currentCategory = Registry.appState.Tools.buildToolState.selectedEntityCategory;

            return Registry.appState.Tools.buildToolState.selectedEntityType switch
            {
                Entity.Type.Room =>
                    Registry.Definitions.Entities.Rooms.Queries.FindByCategory(currentCategory)
                        .Select((definition) =>
                            new UISelectButton.Input()
                            {
                                label = definition.title,
                                value = Room.KeyLabelMap.ValueFromKey(definition.key)
                            }
                        ).ToList(),
                Entity.Type.Furniture =>
                    Registry.Definitions.Entities.Furnitures.Queries.FindByCategory(currentCategory)
                        .Select((definition) =>
                            new UISelectButton.Input()
                            {
                                label = definition.title,
                                value = Furniture.KeyLabelMap.ValueFromKey(definition.key)
                            }
                        ).ToList(),
                Entity.Type.Resident =>
                    Registry.Definitions.Entities.Residents.Queries.FindByCategory(currentCategory)
                        .Select((definition) =>
                            new UISelectButton.Input()
                            {
                                label = definition.title,
                                value = Resident.KeyLabelMap.ValueFromKey(definition.key)
                            }
                        ).ToList(),
                Entity.Type.TransportationItem =>
                    Registry.Definitions.Entities.TransportationItems.Queries.FindByCategory(currentCategory)
                        .Select((definition) =>
                            new UISelectButton.Input()
                            {
                                label = definition.title,
                                value = TransportationItem.KeyLabelMap.ValueFromKey(definition.key)
                            }
                        ).ToList(),
                _ => null
            };
        }

        /*
            Event Handlers
        */
        void OnCategoryButtonClick(string entityCategory)
        {
            Registry.appState.Tools.buildToolState.SetSelectedEntityCategory(entityCategory);
        }

        void OnDefinitionButtonClick(string entityDefinitionLabel)
        {
            Entity.Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(entityDefinitionLabel);
        }

        void OnSelectedEntityCategoryUpdated(string newEntityCategory)
        {
            HighlightSelectedCategoryButton();
            // Build new set of definition buttons
            DestroyDefinitionButtons();
            CreateDefinitionButtons();
            HighlightSelectedDefinitionButton();
        }

        void OnSelectedEntityDefinitionUpdated(EntityDefinition entityDefinition)
        {
            Entity.Type entityType = Registry.appState.Tools.buildToolState.selectedEntityType;

            string label = entityDefinition switch
            {
                RoomDefinition roomDefinition =>
                    Room.KeyLabelMap.ValueFromKey(roomDefinition.key),
                FurnitureDefinition furnitureDefinition =>
                    Furniture.KeyLabelMap.ValueFromKey(furnitureDefinition.key),
                ResidentDefinition residentDefinition =>
                    Resident.KeyLabelMap.ValueFromKey(residentDefinition.key),
                TransportationItemDefinition transportationItemDefinition =>
                    TransportationItem.KeyLabelMap.ValueFromKey(transportationItemDefinition.key),
                _ => null
            };

            HighlightSelectedDefinitionButton();

            // Registry.appState.Tools.buildToolState.SetSelectedEntityDefinition(label);
        }
    }
}
