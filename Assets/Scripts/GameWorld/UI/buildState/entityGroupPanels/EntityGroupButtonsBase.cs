using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using TowerBuilder.Templates;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

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

        protected abstract string categoryButtonsWrapperName { get; }
        protected abstract string templateButtonsWrapperName { get; }

        protected abstract void OnCategoryButtonClick(string categoryValue);
        protected abstract void OnTemplateButtonClick(string templateValue);

        protected abstract List<UISelectButton> GenerateCategoryButtons();
        protected abstract List<UISelectButton> GenerateTemplateButtons();

        public EntityGroupButtonsBase(Transform panelWrapper)
        {
            this.panelWrapper = panelWrapper;

            categoryButtonsWrapper = panelWrapper.Find(categoryButtonsWrapperName);
            templateButtonsWrapper = panelWrapper.Find(templateButtonsWrapperName);

            TransformUtils.DestroyChildren(categoryButtonsWrapper);
            TransformUtils.DestroyChildren(templateButtonsWrapper);
        }

        public void Setup()
        {
            ClearCategoryButtons();
            CreateCategoryButtons();
            HighlightSelectedCategoryButton();

            ClearTemplateButtons();
            CreateTemplateButtons();
            HighlightSelectedTemplateButton();
        }

        public void Teardown()
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
            categoryButtons = GenerateCategoryButtons();
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
            templateButtons = GenerateTemplateButtons();
            foreach (UISelectButton templateButton in templateButtons)
            {
                templateButton.transform.SetParent(this.templateButtonsWrapper, false);
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
