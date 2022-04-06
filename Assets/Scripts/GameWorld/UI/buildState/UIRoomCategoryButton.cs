using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{

    public class UIRoomCategoryButton : MonoBehaviour
    {
        public string roomCategory;
        Button button;
        Text buttonText;

        // TODO - this should change based on category
        Color defaultColor = Color.white;
        Color selectedColor = Color.red;

        public delegate void CategoryButtonEvent(string roomCategory);
        public CategoryButtonEvent onClick;

        public void SetRoomCategory(string roomCategory)
        {
            this.roomCategory = roomCategory;
            buttonText.text = roomCategory;
        }

        public void SetSelected(bool isSelected)
        {
            button.image.color = isSelected ? selectedColor : defaultColor; 
        }

        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);

            buttonText = button.transform.Find("Text").GetComponent<Text>();
        }

        void OnClick()
        {
            if (onClick != null)
            {
                onClick(roomCategory);
            }
        }
    }
}
