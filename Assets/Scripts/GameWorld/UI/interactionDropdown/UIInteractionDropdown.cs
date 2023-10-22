using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIInteractionDropdown : MonoBehaviour
    {
        public bool isOpen { get; private set; }

        bool hasInitialized = false;

        public GameObject uiInteractionDropdownPrefab;

        GameObject contentGameObject;

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
            isOpen = true;
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
            contentGameObject.SetActive(true);
        }

        public void Close()
        {
            isOpen = false;
            contentGameObject.SetActive(false);
        }

        UIInteractionDropdownItem CreateDropdownItem(string label, UIInteractionDropdownItem.OnClick onClick)
        {
            // TODO - instantiate the prefab and everything as well
            GameObject newItem = GameObject.Instantiate(uiInteractionDropdownPrefab);
            return new UIInteractionDropdownItem(label, onClick);
        }

        public static UIInteractionDropdown Find()
        {
            var gameObject = GameObject.Find("InteractionDropdown");
            Debug.Log(gameObject);
            var dropdown = gameObject.GetComponent<UIInteractionDropdown>();
            Debug.Log(dropdown);
            return dropdown;
        }
    }
}