using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UIInteractionDropdown : MonoBehaviour
    {
        public bool isOpen { get; private set; }

        bool hasInitialized = false;

        GameObject contentGameObject;

        public void Start()
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
            gameObject.SetActive(false);
        }

        public void SetItems(UIInteractionDropdownItem.Input[] inputs)
        {

        }
    }
}