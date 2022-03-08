using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;
using UnityEngine.UI;

public class InspectPanelManager : MonoBehaviour
{
    Text inspectText;

    void Awake()
    {
        Registry.Stores.MapUI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnCurrentInspectedRoomUpdated;

        inspectText = transform.Find("InspectText").GetComponent<Text>();
        SetInspectText(Registry.Stores.MapUI.inspectToolSubState.currentInspectedRoom);
    }


    void OnCurrentInspectedRoomUpdated(Room room)
    {
        SetInspectText(room);
    }

    void SetInspectText(Room room)
    {
        if (room == null)
        {
            inspectText.text = "no room selected.";
            return;
        }

        MapRoomDetails roomDetails = TowerBuilder.Stores.Map.Constants.ROOM_DETAILS_MAP[room.roomKey];

        string text = roomDetails.title + "\n";
        text += roomDetails.price + "\n";
        text += string.Join(", ", roomDetails.uses) + "\n";

        inspectText.text = text;
    }
}
