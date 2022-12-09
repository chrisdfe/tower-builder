using System.Collections.Generic;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{

    public abstract class EntityGroupButtonsBase
    {
        protected Transform categoryButtonsWrapper;
        protected Transform definitionButtonsWrapper;

        protected List<UISelectButton> categoryButtons = new List<UISelectButton>();
        protected List<UISelectButton> definitionButtons = new List<UISelectButton>();

        protected Transform panelWrapper;

        protected string selectedCategory = "";
        protected string selectedDefinition = "";

        protected string categoryButtonsWrapperName { get { return "CategoryButtons"; } }
        protected string definitionButtonsWrapperName { get { return "DefinitionButtons"; } }

        protected abstract void OnCategoryButtonClick(string categoryValue);
        protected abstract void OnDefinitionButtonClick(string definitionValue);

        protected abstract List<UISelectButton.Input> GenerateCategoryButtonInputs();
        protected abstract List<UISelectButton.Input> GenerateDefinitionButtonInputs();

        public EntityGroupButtonsBase(Transform panelWrapper)
        {
            this.panelWrapper = panelWrapper;

            categoryButtonsWrapper = panelWrapper.Find(categoryButtonsWrapperName);
            definitionButtonsWrapper = panelWrapper.Find(definitionButtonsWrapperName);

            TransformUtils.DestroyChildren(categoryButtonsWrapper);
            TransformUtils.DestroyChildren(definitionButtonsWrapper);
        }

        public virtual void Setup()
        {
            ClearCategoryButtons();
            CreateCategoryButtons();
            selectedCategory = categoryButtons[0].value;
            HighlightSelectedCategoryButton();

            ClearDefinitionButtons();
            CreateDefinitionButtons();
            HighlightSelectedDefinitionButton();
        }

        public virtual void Teardown()
        {
            DestroyCategoryButtons();
            DestroyDefinitionButtons();
        }

        protected void SetSelectedCategory(string category)
        {
            selectedCategory = category;

            HighlightSelectedCategoryButton();
            // Build new set of definition buttons
            DestroyDefinitionButtons();
            CreateDefinitionButtons();
            HighlightSelectedDefinitionButton();
        }

        protected void SetSelectedDefinition(string definition)
        {
            selectedDefinition = definition;
            HighlightSelectedDefinitionButton();
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
                button.SetSelected(button.value == selectedCategory);
            }
        }

        void ClearCategoryButtons()
        {
            TransformUtils.DestroyChildren(categoryButtonsWrapper);
        }

        void DestroyCategoryButtons()
        {
            foreach (UISelectButton categoryButton in categoryButtons)
            {
                categoryButton.onClick -= OnCategoryButtonClick;
                GameObject.Destroy(categoryButton.gameObject);
            }
        }

        void ClearDefinitionButtons()
        {
            TransformUtils.DestroyChildren(definitionButtonsWrapper);
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
                button.SetSelected(button.value == selectedDefinition);
            }
        }

        void DestroyDefinitionButtons()
        {
            foreach (UISelectButton definitionButton in definitionButtons)
            {
                definitionButton.onClick -= OnDefinitionButtonClick;
                GameObject.Destroy(definitionButton.gameObject);
            }
        }
    }
}
