using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State.Rooms;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI.Components
{
    public class UISelectButtonRow : MonoBehaviour
    {
        public class Input
        {
            public Transform wrapper;
            public List<UISelectButton.Input> buttons;
        }

        public string selectedValue { get; private set; } = "";

        public List<UISelectButton> buttons { get; private set; } = new List<UISelectButton>();

        public delegate void SelectionEvent(string value);
        public SelectionEvent onSelect;

        Transform wrapper;

        public void SetSelectedValue(string value)
        {
            selectedValue = value;
            HighlightSelectedButton();
        }

        void HighlightSelectedButton()
        {
            foreach (UISelectButton button in buttons)
            {
                button.SetSelected(button.value == selectedValue);
            }
        }

        void DestroyCategoryButtons()
        {
            foreach (UISelectButton button in buttons)
            {
                button.onClick -= OnSelectButtonClick;
                GameObject.Destroy(button.gameObject);
            }
        }

        void OnSelectButtonClick(string value)
        {
            SetSelectedValue(value);

            if (onSelect != null)
            {
                onSelect(value);
            }
        }

        void ConsumeInput(Input input)
        {
            buttons = new List<UISelectButton>();
            foreach (UISelectButton.Input buttonInput in input.buttons)
            {
                UISelectButton button = UISelectButton.Create(buttonInput);
                button.onClick += OnSelectButtonClick;
                button.transform.SetParent(transform, false);
                buttons.Add(button);
            }
        }

        public static UISelectButtonRow Create(Input input)
        {
            UISelectButtonRow selectButtonRow = GameObject.Instantiate(GetPrefab()).GetComponent<UISelectButtonRow>();
            selectButtonRow.ConsumeInput(input);
            return selectButtonRow;
        }

        static GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("Prefabs/UI/UISelectButtonRow");
        }
    }
}
