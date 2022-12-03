using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
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
                UISelectButton button = UISelectButton.Create(transform, buttonInput);
                button.onClick += OnSelectButtonClick;
                buttons.Add(button);
            }
        }

        /* 
            Static API
        */
        public static UISelectButtonRow Create(Transform parent, Input input)
        {
            UIManager uiManager = GameWorldFindableCache.Find<UIManager>("UIManager");

            GameObject prefab = uiManager.assetList.FindByKey(UIManager.AssetKey.SelectButton);
            GameObject selectButtonRowGameObject = Instantiate<GameObject>(prefab);

            selectButtonRowGameObject.transform.SetParent(parent, false);

            UISelectButtonRow selectButtonRow = selectButtonRowGameObject.GetComponent<UISelectButtonRow>();

            selectButtonRow.ConsumeInput(input);
            return selectButtonRow;
        }
    }
}
