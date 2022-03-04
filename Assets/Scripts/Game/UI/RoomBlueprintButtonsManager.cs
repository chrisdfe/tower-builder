using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{

    public class RoomBlueprintButtonsManager : MonoBehaviour
    {
        Button NoneButton;
        Button LobbyButton;
        Button OfficeButton;
        Button CondoButton;

        void Awake()
        {
            NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            LobbyButton = transform.Find("LobbyButton").GetComponent<Button>();
            OfficeButton = transform.Find("OfficeButton").GetComponent<Button>();
            CondoButton = transform.Find("CondoButton").GetComponent<Button>();

            NoneButton.onClick.AddListener(OnNoneButtonClick);
            LobbyButton.onClick.AddListener(OnLobbyButtonClick);
            OfficeButton.onClick.AddListener(OnOfficeButtonClick);
            CondoButton.onClick.AddListener(OnCondoButtonClick);
        }

        void OnNoneButtonClick()
        {
            Registry.Stores.MapUI.SetSelectedRoomKey(RoomKey.None);
        }

        void OnLobbyButtonClick()
        {
            Registry.Stores.MapUI.SetSelectedRoomKey(RoomKey.Lobby);
        }

        void OnOfficeButtonClick()
        {
            Registry.Stores.MapUI.SetSelectedRoomKey(RoomKey.Office);
        }

        void OnCondoButtonClick()
        {
            Registry.Stores.MapUI.SetSelectedRoomKey(RoomKey.Condo);
        }
    }
}
