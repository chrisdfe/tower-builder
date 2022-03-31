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

            SelectButton(Registry.Stores.UI.buildToolSubState.selectedRoomKey);
            Registry.Stores.UI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnHallwayButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomKey(RoomKey.Hallway);
        }

        void OnLobbyButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomKey(RoomKey.LargeLobby);
        }

        void OnStairsButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomKey(RoomKey.Stairwell);
        }

        void OnElevatorButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomKey(RoomKey.LargeElevator);
        }

        void OnOfficeButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomKey(RoomKey.Office);
        }

        void OnCondoButtonClick()
        {
            Registry.Stores.UI.buildToolSubState.SetSelectedRoomKey(RoomKey.Condo);
        }

        void OnSelectedRoomKeyUpdated(RoomKey roomKey)
        {
            if (currentButton != null)
            {
                currentButton.image.color = originalColor;
            }

            SelectButton(roomKey);
        }

        void SelectButton(RoomKey roomKey)
        {
            currentButton = GetButtonFor(roomKey);
            currentButton.image.color = PRESSED_COLOR;
        }

        Button GetButtonFor(RoomKey roomKey)
        {
            if (roomKey == RoomKey.Lobby)
            {
                return LobbyButton;
            }

            if (roomKey == RoomKey.Elevator)
            {
                return ElevatorButton;
            }

            if (roomKey == RoomKey.Office)
            {
                return OfficeButton;
            }

            if (roomKey == RoomKey.Condo)
            {
                return CondoButton;
            }

            return HallwayButton;
        }
    }
}
