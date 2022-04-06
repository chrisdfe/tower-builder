using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildStateButtonsManager : MonoBehaviour
    {
        public GameObject roomCategoryButtonPrefab;
        public GameObject roomTemplateButtonPrefab;

        Button HallwayButton;

        Color originalColor;
        Button currentButton;

        List<UIRoomCategoryButton> roomCategoryButtons = new List<UIRoomCategoryButton>();
        List<UIRoomTemplateButton> roomTemplateButtons = new List<UIRoomTemplateButton>();

        Transform roomCategoryButtonsWrapper;
        Transform roomTemplatesButtonsWrapper;

        void Awake()
        {
            roomCategoryButtonsWrapper = transform.Find("RoomCategoryButtons");
            roomTemplatesButtonsWrapper = transform.Find("RoomTemplateButtons");

            Debug.Log(roomCategoryButtonsWrapper);
            Debug.Log(roomTemplatesButtonsWrapper);

            SetupRoomCategoryButtons();
            ResetRoomTemplatesButtons();

            Registry.Stores.UI.buildToolSubState.onSelectedRoomCategoryUpdated += OnSelectedRoomCategoryUpdated;
            Registry.Stores.UI.buildToolSubState.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        void OnCategoryButtonClick(string roomCategory)
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomCategory(roomCategory);
        }

        void OnTemplateButtonClick(RoomTemplate roomTemplate)
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate(roomTemplate);
        }

        void OnSelectedRoomCategoryUpdated(string roomCategory)
        {
            SetSelectedRoomCategoryButton();
            ResetRoomTemplatesButtons();
        }

        void OnSelectedRoomTemplateUpdated(RoomTemplate roomTemplate)
        {
            SetSelectedRoomTemplateButton();
        }

        void SetSelectedRoomTemplateButton()
        {
            RoomTemplate selectedTemplate = Registry.Stores.UI.buildToolSubState.selectedRoomTemplate;
            foreach (UIRoomTemplateButton roomTemplateButton in roomTemplateButtons)
            {
                roomTemplateButton.SetSelected(roomTemplateButton.roomTemplate.key == selectedTemplate.key);
            }
        }

        void SetSelectedRoomCategoryButton()
        {
            string currentCategory = Registry.Stores.UI.buildToolSubState.selectedRoomCategory;

            foreach (UIRoomCategoryButton roomCategoryButton in roomCategoryButtons)
            {
                roomCategoryButton.SetSelected(roomCategoryButton.roomCategory == currentCategory);
            }
        }

        void SetupRoomCategoryButtons()
        {
            DestroyRoomCategoryButtons();

            List<string> allRoomCategories = GetAllRoomCategories();

            foreach (string category in allRoomCategories)
            {
                GameObject roomCategoryButtonGameObject = Instantiate(roomCategoryButtonPrefab, roomCategoryButtonsWrapper);
                UIRoomCategoryButton categoryButton = roomCategoryButtonGameObject.GetComponent<UIRoomCategoryButton>();

                categoryButton.onClick += OnCategoryButtonClick;
                categoryButton.SetRoomCategory(category);
                roomCategoryButtons.Add(categoryButton);
            }

            SetSelectedRoomCategoryButton();
        }

        void ResetRoomTemplatesButtons()
        {
            DestroyRoomTemplateButtons();

            List<RoomTemplate> currentRoomTemplates = GetRoomTemplatesForCurrentCategory();
            RoomTemplate currentTemplate = Registry.Stores.UI.buildToolSubState.selectedRoomTemplate;

            foreach (RoomTemplate roomTemplate in currentRoomTemplates)
            {
                GameObject roomTemplateButtonGameObject = GameObject.Instantiate(roomTemplateButtonPrefab, roomTemplatesButtonsWrapper);
                UIRoomTemplateButton roomTemplateButton = roomTemplateButtonGameObject.GetComponent<UIRoomTemplateButton>();
                roomTemplateButton.SetSelected(roomTemplate.key == currentTemplate.key);

                roomTemplateButton.onClick += OnTemplateButtonClick;
                roomTemplateButton.SetRoomTemplate(roomTemplate);
                roomTemplateButtons.Add(roomTemplateButton);
            }
        }

        void DestroyRoomCategoryButtons()
        {
            foreach (Transform child in roomCategoryButtonsWrapper.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        void DestroyRoomTemplateButtons()
        {
            foreach (Transform child in roomTemplatesButtonsWrapper.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            roomTemplateButtons = new List<UIRoomTemplateButton>();
        }

        List<RoomTemplate> GetRoomTemplatesForCurrentCategory()
        {
            string currentCategory = Registry.Stores.UI.buildToolSubState.selectedRoomCategory;
            return Constants.ROOM_DEFINITIONS.FindAll(roomTemplate => roomTemplate.category == currentCategory).ToList();
        }

        List<string> GetAllRoomCategories()
        {
            List<string> result = new List<string>();

            foreach (RoomTemplate roomTemplate in Constants.ROOM_DEFINITIONS)
            {
                if (!result.Contains(roomTemplate.category))
                {
                    result.Add(roomTemplate.category);
                }
            }

            return result;
        }
    }
}
