using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIInteractionDropdown : MonoBehaviour
    {
        public bool isOpen { get; private set; }

        bool hasInitialized = false;

        GameObject contentGameObject;

        List<UIInteractionDropdownItem> items;

        public void Awake()
        {
            contentGameObject = transform.Find("Content").gameObject;
        }

        public void Toggle()
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
            contentGameObject.SetActive(true);
            isOpen = true;

            // For debug purposes
            SetItems(new List<UIInteractionDropdownItem.Input>() {
                new UIInteractionDropdownItem.Input() {
                    label = "test",
                    onClick = TestClickCallback
                },
                new UIInteractionDropdownItem.Input() {
                    label = "test 2",
                    onClick = TestClickCallback
                },
            });

            Vector3 screenMousePosition = Input.mousePosition;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            CanvasScaler canvasScaler = UIManager.Find().canvasScaler;
            Vector2 scaleFactor = new Vector2(
                canvasScaler.transform.localScale.x,
                canvasScaler.transform.localScale.y
            );
            rectTransform.position = new Vector3(
                screenMousePosition.x + (rectTransform.rect.width * scaleFactor.x / 2),
                screenMousePosition.y - (rectTransform.rect.height * scaleFactor.y / 2),
                0
            );
        }

        public void Close()
        {
            isOpen = false;
            contentGameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void SetItems(List<UIInteractionDropdownItem.Input> inputs)
        {
            TransformUtils.DestroyChildren(contentGameObject.transform);
            items = new List<UIInteractionDropdownItem>();

            foreach (var input in inputs)
            {
                var wrappedInput = new UIInteractionDropdownItem.Input()
                {
                    label = input.label,
                    onClick = () => WrapOnClick(input.onClick)
                };

                var dropdownItem = UIInteractionDropdownItem.Create(wrappedInput);

                // Preserve the scale that gets messed up during SetParent
                // TODO - move into a TransformUtils helper
                var originalScale = dropdownItem.transform.localScale;
                dropdownItem.transform.SetParent(contentGameObject.transform);
                dropdownItem.transform.localScale = originalScale;
                items.Append(dropdownItem);
            }
        }

        public void SetItemsAndOpen(List<UIInteractionDropdownItem.Input> inputs)
        {
            Open();
            SetItems(inputs);
        }

        void WrapOnClick(UIInteractionDropdownItem.OnClick onClick)
        {
            onClick();
            Close();
        }

        void TestClickCallback()
        {
            Debug.Log("Click.");
        }
    }
}