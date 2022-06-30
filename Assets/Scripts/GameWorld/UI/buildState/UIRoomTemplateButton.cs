using System.Collections;
using System.Collections.Generic;
using TowerBuilder.State.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{

    public class UIRoomTemplateButton : MonoBehaviour
    {
        public RoomTemplate roomTemplate;
        Button button;
        Text buttonText;

        // TODO - this should change based on category
        Color defaultColor = Color.white;
        Color selectedColor = Color.red;

        public delegate void TemplateButtonEvent(RoomTemplate roomTemplate);
        public TemplateButtonEvent onClick;

        public void SetRoomTemplate(RoomTemplate roomTemplate)
        {
            this.roomTemplate = roomTemplate;
            buttonText.text = roomTemplate.title;
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
                onClick(roomTemplate);
            }
        }
    }
}
