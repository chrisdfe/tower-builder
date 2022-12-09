using System.Collections.Generic;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{

    public abstract class EntityGroupButtonsBase
    {
        protected Transform categoryButtonsWrapper;
        protected Transform templateButtonsWrapper;

        protected List<UISelectButton> categoryButtons = new List<UISelectButton>();
        protected List<UISelectButton> templateButtons = new List<UISelectButton>();

        protected Transform panelWrapper;

        protected string selectedCategory = "";
        protected string selectedTemplate = "";

        protected string categoryButtonsWrapperName { get { return "CategoryButtons"; } }
        protected string templateButtonsWrapperName { get { return "TemplateButtons"; } }

        protected abstract void OnCategoryButtonClick(string categoryValue);
        protected abstract void OnTemplateButtonClick(string templateValue);

        protected abstract List<UISelectButton.Input> GenerateCategoryButtonInputs();
        protected abstract List<UISelectButton.Input> GenerateTemplateButtonInputs();

        public EntityGroupButtonsBase(Transform panelWrapper)
        {
            this.panelWrapper = panelWrapper;

            categoryButtonsWrapper = panelWrapper.Find(categoryButtonsWrapperName);
            templateButtonsWrapper = panelWrapper.Find(templateButtonsWrapperName);

            TransformUtils.DestroyChildren(categoryButtonsWrapper);
            TransformUtils.DestroyChildren(templateButtonsWrapper);
        }

        public virtual void Setup()
        {
            ClearCategoryButtons();
            CreateCategoryButtons();
            selectedCategory = categoryButtons[0].value;
            HighlightSelectedCategoryButton();

            ClearTemplateButtons();
            CreateTemplateButtons();
            HighlightSelectedTemplateButton();
        }

        public virtual void Teardown()
        {
            DestroyCategoryButtons();
            DestroyTemplateButtons();
        }

        protected void SetSelectedCategory(string category)
        {
            selectedCategory = category;

            HighlightSelectedCategoryButton();
            // Build new set of template buttons
            DestroyTemplateButtons();
            CreateTemplateButtons();
            HighlightSelectedTemplateButton();
        }

        protected void SetSelectedTemplate(string template)
        {
            selectedTemplate = template;
            HighlightSelectedTemplateButton();
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

        void ClearTemplateButtons()
        {
            TransformUtils.DestroyChildren(templateButtonsWrapper);
        }

        void CreateTemplateButtons()
        {
            templateButtons = UISelectButton.CreateButtonListFromInputList(templateButtonsWrapper, GenerateTemplateButtonInputs());

            foreach (UISelectButton templateButton in templateButtons)
            {
                templateButton.onClick += OnTemplateButtonClick;
            }
        }

        void HighlightSelectedTemplateButton()
        {
            foreach (UISelectButton button in templateButtons)
            {
                button.SetSelected(button.value == selectedTemplate);
            }
        }

        void DestroyTemplateButtons()
        {
            foreach (UISelectButton templateButton in templateButtons)
            {
                templateButton.onClick -= OnTemplateButtonClick;
                GameObject.Destroy(templateButton.gameObject);
            }
        }
    }
}
