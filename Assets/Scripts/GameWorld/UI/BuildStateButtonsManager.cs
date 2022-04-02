using TowerBuilder.Stores;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildStateButtonsManager : MonoBehaviour
    {
        static Color PRESSED_COLOR = Color.red;

        Button HallwayButton;
        Button LobbyButton;
        Button StairsButton;
        Button ElevatorButton;
        Button OfficeButton;
        Button CondoButton;

        Color originalColor;
        Button currentButton;

        void Awake()
        {
            HallwayButton = transform.Find("HallwayButton").GetComponent<Button>();
            LobbyButton = transform.Find("LobbyButton").GetComponent<Button>();
            StairsButton = transform.Find("StairsButton").GetComponent<Button>();
            ElevatorButton = transform.Find("ElevatorButton").GetComponent<Button>();
            OfficeButton = transform.Find("OfficeButton").GetComponent<Button>();
            CondoButton = transform.Find("CondoButton").GetComponent<Button>();

            HallwayButton.onClick.AddListener(OnHallwayButtonClick);
            LobbyButton.onClick.AddListener(OnLobbyButtonClick);
            StairsButton.onClick.AddListener(OnStairsButtonClick);
            ElevatorButton.onClick.AddListener(OnElevatorButtonClick);
            OfficeButton.onClick.AddListener(OnOfficeButtonClick);
            CondoButton.onClick.AddListener(OnCondoButtonClick);

            originalColor = LobbyButton.colors.normalColor;

            SelectButton(Registry.Stores.UI.buildToolSubState.selectedRoomTemplate);
            Registry.Stores.UI.buildToolSubState.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        void OnHallwayButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate("Hallway");
        }

        void OnLobbyButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate("LargeLobby");
        }

        void OnStairsButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate("Stairwell");
        }

        void OnElevatorButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate("LargeElevator");
        }

        void OnOfficeButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate("Office");
        }

        void OnCondoButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomTemplate("Condo");
        }

        void OnSelectedRoomTemplateUpdated(RoomTemplate roomTemplate)
        {
            if (currentButton != null)
            {
                currentButton.image.color = originalColor;
            }

            SelectButton(roomTemplate);
        }

        void SelectButton(RoomTemplate roomTemplate)
        {
            currentButton = GetButtonFor(roomTemplate);

            if (currentButton != null)
            {
                currentButton.image.color = PRESSED_COLOR;
            }
        }

        Button GetButtonFor(RoomTemplate roomTemplate)
        {
            if (roomTemplate == null)
            {
                return null;
            }

            string roomKey = roomTemplate.key;

            if (roomKey == "Lobby")
            {
                return LobbyButton;
            }

            if (roomKey == "Stairwell")
            {
                return StairsButton;
            }

            if (roomKey == "Elevator")
            {
                return ElevatorButton;
            }

            if (roomKey == "Office")
            {
                return OfficeButton;
            }

            if (roomKey == "Condo")
            {
                return CondoButton;
            }

            if (roomKey == "Hallway")
            {
                return HallwayButton;
            }

            return null;
        }
    }
}
