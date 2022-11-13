using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI.Components
{
    public class UISelectButton : MonoBehaviour
    {
        public class Input
        {
            public string label;
            public string value;
        }

        public string label { get; private set; }
        public string value { get; private set; }

        Button button;
        Text buttonText;

        static Color TEAL = ColorUtils.ColorFromHex("#033A34");
        Color defaultBackgroundColor = Color.white;
        Color defaultTextColor = TEAL;

        Color selectedBackgroundColor = TEAL;
        Color selectedTextColor = Color.white;

        public delegate void ClickEvent(string value);
        public ClickEvent onClick;

        public void SetSelected(bool isSelected)
        {
            button.image.color = isSelected ? selectedBackgroundColor : defaultBackgroundColor;
            buttonText.color = isSelected ? selectedTextColor : defaultTextColor;
        }

        public void ConsumeInput(Input input)
        {
            this.label = input.label;
            this.value = input.value;
        }

        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);

            buttonText = button.transform.Find("Text").GetComponent<Text>();
        }

        void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        void RenderText()
        {
            buttonText.text = label;
        }

        void OnClick()
        {
            if (onClick != null)
            {
                onClick(value);
            }
        }

        /* 
            Static API
        */
        public static UISelectButton Create(Input input)
        {
            UISelectButton button = Instantiate<GameObject>(UISelectButton.GetPrefab()).GetComponent<UISelectButton>();
            button.ConsumeInput(input);
            button.RenderText();
            return button;
        }

        public static List<UISelectButton> CreateButtonListFromInputList(List<Input> inputList)
        {
            List<UISelectButton> result = new List<UISelectButton>();

            foreach (UISelectButton.Input input in inputList)
            {
                UISelectButton selectButton = UISelectButton.Create(new UISelectButton.Input() { label = input.label, value = input.value });
                result.Add(selectButton);
            }

            return result;
        }

        static GameObject GetPrefab()
        {
            return Resources.Load<GameObject>("Prefabs/UI/UISelectButton");
        }
    }
}
