using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class InspectPanelManager : MonoBehaviour
    {
        Text inspectText;

        void Awake()
        {
            Registry.appState.UI.inspectToolSubState.onCurrentInspectedRoomUpdated += OnCurrentInspectedRoomUpdated;

            inspectText = transform.Find("InspectText").GetComponent<Text>();
            SetInspectText(Registry.appState.UI.inspectToolSubState.currentInspectedRoom);
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

            RoomConnections roomConnections = Registry.appState.Rooms.roomConnections.FindConnectionsForRoom(room);

            string text = room + "\n";
            text += room.title + "\n";
            text += room.price + "\n";
            text += "\n";

            text += $"{roomConnections.items.Count} Connections\n";
            // text += string.Join(", ", room.roomTemplate.uses) + "\n";
            foreach (RoomConnection connection in roomConnections.items)
            {
                text += connection + "\n";
            }

            inspectText.text = text;
        }
    }
}