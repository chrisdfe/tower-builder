using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class InspectPanelManager : MonoBehaviour
    {
        Text inspectIndexText;
        Text inspectText;
        Transform actionButtonsWrapper;

        List<UISelectButton> actionButtons = new List<UISelectButton>();

        void Awake()
        {
            inspectText = transform.Find("InspectText").GetComponent<Text>();
            inspectIndexText = transform.Find("InspectIndexText").GetComponent<Text>();
            actionButtonsWrapper = transform.Find("ActionButtonsWrapper");

            SetText();

            Registry.appState.Tools.inspectToolState.events.onInspectedEntityListUpdated += OnInspectedEntityListUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        void OnInspectedEntityListUpdated(EntityList entityList)
        {
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
            SetActionButtons();
        }

        void SetInspectIndexText()
        {
            EntityList inspectedEntityList = Registry.appState.Tools.inspectToolState.inspectedEntityList;
            int index = Registry.appState.Tools.inspectToolState.inspectedEntityIndex;

            string text = $"entityList: {inspectedEntityList.Count}\n"
                + $"index: {index}";

            inspectIndexText.text = text;
        }

        void SetInspectText()
        {
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
                case ResidentEntity residentEntity:
                    SetInspectedResidentText(residentEntity.resident);
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
            string text = "Furniture\n"
                + $"   name: {furniture}\n"
                + $"   condition: {furniture.condition}";

            inspectText.text = text;
        }

        void SetInspectedRoomBlockText(RoomCells roomBlock)
        {
            string text = "RoomBlock";

            inspectText.text = text;
        }

        void SetInspectedRoomEntityText(Room room)
        {
            RoomConnectionList roomConnections = Registry.appState.Rooms.roomConnectionList.FindConnectionsForRoom(room);

            string text = "Room\n"
            + $"    name: {room}\n"
            + $"    title: {room.title}\n"
            + $"    price: {room.price}\n";

            text += "\n";
            text += $"{roomConnections.Count} Connection{(roomConnections.Count == 1 ? "" : "s")}\n";

            roomConnections.ForEach(roomConnection =>
            {
                text += $"    {roomConnection.nodeA.room} - {roomConnection.nodeB.room}\n";
            });

            inspectText.text = text;
        }

        void SetInspectedResidentText(Resident resident)
        {
            ResidentBehavior residentBehavior = Registry.appState.ResidentBehaviors.queries.FindByResident(resident);

            string text = "Resident"
            + $"   name: {resident}\n"
            + $"   state: {residentBehavior.currentState}\n";

            inspectText.text = text;
        }

        void SetActionButtons()
        {
            DestroyActionButtons();

            EntityBase inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;

            switch (inspectedEntity)
            {
                case RoomEntity roomEntity:
                    break;
                case RoomBlockEntity roomBlockEntity:
                    break;
                case FurnitureEntity furnitureEntity:
                    CreateFurnitureActionButtons(furnitureEntity.furniture);
                    break;
                case ResidentEntity residentEntity:
                    CreateResidentActionButtons(residentEntity.resident);
                    break;
            }
        }

        void DestroyActionButtons()
        {
            actionButtons = new List<UISelectButton>();
            TransformUtils.DestroyChildren(actionButtonsWrapper);
        }

        void CreateFurnitureActionButtons(Furniture furniture)
        {
            UISelectButton removeButton = UISelectButton.Create(new UISelectButton.Input() { label = "delete", value = "delete" });
            removeButton.onClick += (value) => { Registry.appState.Furnitures.RemoveFurniture(furniture); };
            removeButton.transform.SetParent(actionButtonsWrapper);
            removeButton.transform.localScale = Vector3.one;

            float wrapperHeight = actionButtonsWrapper.GetComponent<RectTransform>().rect.height;
            RectTransform removeButtonRectTransform = removeButton.GetComponent<RectTransform>();
            removeButtonRectTransform.sizeDelta = new Vector2(
                removeButtonRectTransform.rect.width,
                wrapperHeight
            );
        }

        void CreateResidentActionButtons(Resident resident)
        {
            UISelectButton removeButton = UISelectButton.Create(new UISelectButton.Input() { label = "delete", value = "delete" });
            removeButton.onClick += (value) => { Registry.appState.Residents.RemoveResident(resident); };
            removeButton.transform.SetParent(actionButtonsWrapper);
            removeButton.transform.localScale = Vector3.one;

            float wrapperHeight = actionButtonsWrapper.GetComponent<RectTransform>().rect.height;
            RectTransform removeButtonRectTransform = removeButton.GetComponent<RectTransform>();
            removeButtonRectTransform.sizeDelta = new Vector2(
                removeButtonRectTransform.rect.width,
                wrapperHeight
            );
        }
    }
}