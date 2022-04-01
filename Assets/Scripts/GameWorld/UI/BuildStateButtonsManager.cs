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

            SelectButton(Registry.Stores.UI.buildToolSubState.selectedRoomDetails);
            Registry.Stores.UI.buildToolSubState.onSelectedRoomDetailsUpdated += OnSelectedRoomDetailsUpdated;
        }

        void OnHallwayButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomDetails("Hallway");
        }

        void OnLobbyButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomDetails("LargeLobby");
        }

        void OnStairsButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomDetails("Stairwell");
        }

        void OnElevatorButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomDetails("LargeElevator");
        }

        void OnOfficeButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomDetails("Office");
        }

        void OnCondoButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomDetails("Condo");
        }

        void OnSelectedRoomDetailsUpdated(RoomDetails roomDetails)
        {
            if (currentButton != null)
            {
                currentButton.image.color = originalColor;
            }

            SelectButton(roomDetails);
        }

        void SelectButton(RoomDetails roomDetails)
        {
            currentButton = GetButtonFor(roomDetails);

            if (currentButton != null)
            {
                currentButton.image.color = PRESSED_COLOR;
            }
        }

        Button GetButtonFor(RoomDetails roomDetails)
        {
            if (roomDetails == null)
            {
                return null;
            }

            string roomKey = roomDetails.key;

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
