using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class InspectPanelManager : MonoBehaviour
    {
        Text inspectIndexText;
        Text inspectText;

        void Awake()
        {
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;

            inspectText = transform.Find("InspectText").GetComponent<Text>();
            inspectIndexText = transform.Find("InspectIndexText").GetComponent<Text>();

            SetText();
        }


        void OnCurrentSelectedEntityUpdated(EntityBase entity)
        {
            SetText();
        }

        void SetText()
        {
            SetInspectIndexText();
            SetInspectText();
        }

        void SetInspectIndexText()
        {
            int index = Registry.appState.Tools.inspectToolState.inspectedEntityIndex;
            inspectIndexText.text = $"index: {index}";
        }

        void SetInspectText()
        {
            Debug.Log(Registry.appState.Tools.inspectToolState.inspectedEntityIndex);
            Debug.Log(Registry.appState.Tools.inspectToolState.inspectedEntity);
            EntityBase inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;

            switch (inspectedEntity)
            {
                case RoomEntity roomEntity:
                    SetInspectedRoomEntityText(roomEntity.room);
                    break;
                case RoomBlockEntity roomBlockEntity:
                    SetInspectedRoomBlockText(roomBlockEntity.roomBlock);
                    break;
                case FurnitureEntity furnitureEntity:
                    SetInspectedFurnitureText(furnitureEntity.furniture);
                    break;
                default:
                    SetNullInspectedText();
                    break;
            }
        }

        void SetNullInspectedText()
        {
            inspectText.text = "---";
        }

        void SetInspectedFurnitureText(Furniture furniture)
        {
            string text = "Furniture";

            inspectText.text = text;
        }

        void SetInspectedRoomBlockText(RoomCells roomBlock)
        {
            string text = "RoomBlock";

            inspectText.text = text;
        }

        void SetInspectedRoomEntityText(Room room)
        {
            RoomConnections roomConnections = Registry.appState.Rooms.roomConnections.FindConnectionsForRoom(room);

            string text = "Room\n"
            + $"name: {room}\n"
            + $"title: {room.title}\n"
            + $"price: {room.price}\n";

            text += $"{roomConnections.connections.Count} Connection{(roomConnections.connections.Count == 1 ? "" : "s")}\n";

            foreach (RoomConnection connection in roomConnections.connections)
            {
                text += $"    {connection.nodeA.room} - {connection.nodeB.room}\n";
            }

            inspectText.text = text;
        }
    }
}