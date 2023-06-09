using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    // TODO - this is basically the same thing as UIButtonRow
    public abstract class BuildToolStateButtonsRowBase : MonoBehaviour, ISetupable
    {
        protected List<UISelectButton> buttons = new List<UISelectButton>();

        public BuildToolStateButtonsRowBase() { }

        public virtual void Setup()
        {
            TransformUtils.DestroyChildren(transform);
            CreateButtons();
            HighlightSelectedButton();
        }

        public virtual void Teardown()
        {
            DestroyButtons();
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            DestroyButtons();
            CreateButtons();
        }

        public void HighlightSelectedButton()
        {
            foreach (UISelectButton button in buttons)
            {
                button.SetSelected(ButtonShouldBeSelected(button));
            }
        }

        public abstract List<UISelectButton.Input> CreateButtonInputs();

        public abstract bool ButtonShouldBeSelected(UISelectButton button);

        public abstract void OnButtonClick(string value);

        void CreateButtons()
        {
            List<UISelectButton.Input> inputs = CreateButtonInputs();
            buttons = UISelectButton.CreateButtonListFromInputList(transform, inputs);

            foreach (UISelectButton button in buttons)
            {
                button.onClick += OnButtonClick;
            }

            HighlightSelectedButton();
        }

        void DestroyButtons()
        {
            foreach (UISelectButton button in buttons)
            {
                button.onClick -= OnButtonClick;
                GameObject.Destroy(button.gameObject);
            }

            TransformUtils.DestroyChildren(transform);
        }
    }
}