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
            text += room.roomTemplate.title + "\n";
            text += room.roomTemplate.price + "\n";
            text += "\n";

            text += $"{roomConnections.connections.Count} Connections\n";
            // text += string.Join(", ", room.roomTemplate.uses) + "\n";
            foreach (RoomConnection connection in roomConnections.connections)
            {
                // Room otherRoom = connection.GetConnectedRoom(room);
                text += connection + "\n";
            }

            // foreach (RoomCell roomCell in room.roomCells.cells)
            // {
            //     text += roomCell.coordinates + "\n";
            // }

            inspectText.text = text;
        }
    }
}