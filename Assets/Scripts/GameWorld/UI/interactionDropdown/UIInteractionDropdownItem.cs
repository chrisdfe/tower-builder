using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIInteractionDropdownItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public delegate void OnClick();

        public class Input
        {
            public string label;
            public OnClick onClick;
        }

        GameObject contentGameObject;
        GameObject labelGameObject;
        Image contentBgImage;
        Color contentBgImageOriginalColor;
        Text labelText;

        string label;
        OnClick onClick;

        public void Awake()
        {
            contentGameObject = transform.Find("Content").gameObject;
            labelGameObject = contentGameObject.transform.Find("Label").gameObject;

            contentBgImage = contentGameObject.GetComponent<Image>();
            contentBgImageOriginalColor = new Color(contentBgImage.color.r, contentBgImage.color.b, contentBgImage.color.g);
            labelText = labelGameObject.GetComponent<Text>();

            UpdateText();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            contentBgImage.color = new Color(contentBgImageOriginalColor.r, contentBgImageOriginalColor.g, contentBgImageOriginalColor.b, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            contentBgImage.color = new Color(contentBgImageOriginalColor.r, contentBgImageOriginalColor.g, contentBgImageOriginalColor.b, 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick();
        }

        public void ConsumeInput(Input input)
        {
            onClick = input.onClick;
            label = input.label;

            UpdateText();
        }

        void UpdateText()
        {
            labelText.text = label;
        }

        public static UIInteractionDropdownItem Create(Input input)
        {
            var prefab = DropdownsManager.Find().assetList.ValueFromKey("InteractionDropdownItem");
            var gameObject = Instantiate(prefab);
            var interactionDropdownItem = gameObject.GetComponent<UIInteractionDropdownItem>();
            interactionDropdownItem.ConsumeInput(input);
            return interactionDropdownItem;
        }
    }
}