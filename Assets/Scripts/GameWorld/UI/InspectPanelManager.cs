using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
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

            RoomConnections roomConnections = Registry.Stores.Map.roomConnections.FindConnectionsForRoom(room);

            string text = room + "\n";
            text += room.roomDetails.title + "\n";
            text += room.roomDetails.price + "\n";
            text += "\n";

            text += $"{roomConnections.connections.Count} Connections\n";
            // text += string.Join(", ", room.roomDetails.uses) + "\n";
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