using System.Collections;
using System.Collections.Generic;
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

        void Awake()
        {
            NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            LobbyButton = transform.Find("LobbyButton").GetComponent<Button>();
            OfficeButton = transform.Find("OfficeButton").GetComponent<Button>();

            NoneButton.onClick.AddListener(OnNoneButtonClick);
            LobbyButton.onClick.AddListener(OnLobbyButtonClick);
            OfficeButton.onClick.AddListener(OnOfficeButtonClick);
        }

        void OnNoneButtonClick()
        {
            SetSelectedRoomType(RoomKey.None);
        }

        void OnLobbyButtonClick()
        {

            SetSelectedRoomType(RoomKey.Lobby);
        }

        void OnOfficeButtonClick()
        {
            SetSelectedRoomType(RoomKey.Office);
        }

        void SetSelectedRoomType(RoomKey roomKey)
        {
            MapUIStore.Mutations.SetSelectedRoomKey(roomKey);
        }
    }
}
