using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.Game.UI
{
    public class RoomBlueprintButtonsManager : MonoBehaviour
    {
        static Color PRESSED_COLOR = Color.red;

        Button NoneButton;
        Button LobbyButton;
        Button ElevatorButton;
        Button OfficeButton;
        Button CondoButton;

        Color originalColor;
        Button currentButton;

        void Awake()
        {
            NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            LobbyButton = transform.Find("LobbyButton").GetComponent<Button>();
            ElevatorButton = transform.Find("ElevatorButton").GetComponent<Button>();
            OfficeButton = transform.Find("OfficeButton").GetComponent<Button>();
            CondoButton = transform.Find("CondoButton").GetComponent<Button>();

            NoneButton.onClick.AddListener(OnNoneButtonClick);
            LobbyButton.onClick.AddListener(OnLobbyButtonClick);
            ElevatorButton.onClick.AddListener(OnElevatorButtonClick);
            OfficeButton.onClick.AddListener(OnOfficeButtonClick);
            CondoButton.onClick.AddListener(OnCondoButtonClick);

            originalColor = LobbyButton.colors.normalColor;

            SelectButton(Registry.Stores.MapUI.buildToolSubState.selectedRoomKey);
            Registry.Stores.MapUI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void OnNoneButtonClick()
        {
            Registry.Stores.MapUI.buildToolSubState.SetSelectedRoomKey(RoomKey.None);
        }

        void OnLobbyButtonClick()
        {
            Registry.Stores.MapUI.buildToolSubState.SetSelectedRoomKey(RoomKey.Lobby);
        }

        void OnElevatorButtonClick()
        {
            Registry.Stores.MapUI.buildToolSubState.SetSelectedRoomKey(RoomKey.Elevator);
        }

        void OnOfficeButtonClick()
        {
            Registry.Stores.MapUI.buildToolSubState.SetSelectedRoomKey(RoomKey.Office);
        }

        void OnCondoButtonClick()
        {
            Registry.Stores.MapUI.buildToolSubState.SetSelectedRoomKey(RoomKey.Condo);
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

            return NoneButton;
        }
    }
}
