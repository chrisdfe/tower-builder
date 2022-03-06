using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{

    public class RoomBlueprintButtonsManager : MonoBehaviour
    {
        Button NoneButton;
        Button LobbyButton;
        Button ElevatorButton;
        Button OfficeButton;
        Button CondoButton;

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
    }
}
