using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Residents.Attributes;
using TowerBuilder.DataTypes.Entities.Residents.Behaviors;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
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

            Setup();
        }

        public void Setup()
        {
            Registry.appState.ResidentAttributesWrappers.events.onItemsUpdated += OnResidentAttributeWrappersUpdated;

            Registry.appState.Tools.inspectToolState.events.onInspectedEntityListUpdated += OnInspectedEntityListUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            Registry.appState.ResidentAttributesWrappers.events.onItemsUpdated -= OnResidentAttributeWrappersUpdated;

            Registry.appState.Tools.inspectToolState.events.onInspectedEntityListUpdated -= OnInspectedEntityListUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /* 
            Internals
        */
        void SetText()
        {
            SetInspectIndexText();
            SetInspectText();
            SetActionButtons();
        }

        void SetInspectIndexText()
        {
            ListWrapper<Entity> inspectedEntityList = Registry.appState.Tools.inspectToolState.inspectedEntityList;
            int index = Registry.appState.Tools.inspectToolState.inspectedEntityIndex;

            string text = $"entityList: {inspectedEntityList.Count}\n"
                + $"index: {index}";

            inspectIndexText.text = text;
        }

        void SetInspectText()
        {
            Entity inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;

            if (inspectedEntity == null) return;

            inspectText.text = $"{inspectedEntity.typeLabel} - {inspectedEntity.ToString()}\n";

            switch (inspectedEntity)
            {
                case Room roomEntity:
                    SetInspectedRoomEntityText(roomEntity);
                    break;
                case Furniture furnitureEntity:
                    SetInspectedFurnitureText(furnitureEntity);
                    break;
                case Resident residentEntity:
                    SetInspectedResidentText(residentEntity);
                    break;
                case TransportationItem transportationItemEntity:
                    SetInspectedTransportationItemText(transportationItemEntity);
                    break;
            }
        }

        void SetInspectedFurnitureText(Furniture furniture)
        {
            string text = $"   name: {furniture}\n";

            inspectText.text += text;
        }

        void SetInspectedRoomEntityText(Room room)
        {
            string text =
              $"    name: {room}\n"
            + $"    title: {room.definition.title}\n"
            + $"    price: {room.price}\n"
            + $"    cells: {room.cellCoordinatesList.Count}\n"
            + $"    blocks: {room.blocksList.Count}\n";

            inspectText.text += text;
        }

        void SetInspectedResidentText(Resident resident)
        {
            ResidentBehavior residentBehavior = Registry.appState.ResidentBehaviors.queries.FindByResident(resident);

            ResidentAttributesWrapper residentAttributesWrapper = Registry.appState.ResidentAttributesWrappers.queries.FindByResident(resident);

            string text =
              $"   name: {resident}\n";

            if (residentBehavior != null)
            {
                text += $"   state: {residentBehavior.currentState}\n";
            }

            if (residentAttributesWrapper != null)
            {
                text += "    attributes:\n";
                residentAttributesWrapper.attributes.ForEach((attribute) =>
                {
                    text += $"        {attribute.key}: {attribute.value}\n";
                });
            }

            inspectText.text += text;
        }

        void SetInspectedTransportationItemText(TransportationItem transportationItem)
        {
            if (transportationItem == null) return;

            string text = $"   name: {transportationItem}\n";

            inspectText.text += text;
        }

        void SetActionButtons()
        {
            DestroyActionButtons();

            Entity inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;

            switch (inspectedEntity)
            {
                case Room roomEntity:
                    break;
                case Furniture furnitureEntity:
                    CreateFurnitureActionButtons(furnitureEntity as Furniture);
                    break;
                case Resident residentEntity:
                    CreateResidentActionButtons(residentEntity as Resident);
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
            UISelectButton removeButton = UISelectButton.Create(
                actionButtonsWrapper,
                new UISelectButton.Input() { label = "delete", value = "delete" }
            );
            removeButton.onClick += (value) => { Registry.appState.Entities.Furnitures.Remove(furniture); };
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
            UISelectButton removeButton = UISelectButton.Create(
                actionButtonsWrapper,
                new UISelectButton.Input() { label = "delete", value = "delete" }
            );
            removeButton.onClick += (value) => { Registry.appState.Entities.Residents.Remove(resident); };
            removeButton.transform.localScale = Vector3.one;

            float wrapperHeight = actionButtonsWrapper.GetComponent<RectTransform>().rect.height;
            RectTransform removeButtonRectTransform = removeButton.GetComponent<RectTransform>();
            removeButtonRectTransform.sizeDelta = new Vector2(
                removeButtonRectTransform.rect.width,
                wrapperHeight
            );
        }

        /* 
            Event Handlers
        */
        void OnInspectedEntityListUpdated(ListWrapper<Entity> entityList)
        {
            SetText();
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            SetText();
        }

        void OnResidentAttributeWrappersUpdated(ResidentAttributesWrapperList residentAttributesWrappers)
        {
            foreach (ResidentAttributesWrapper residentAttributesWrapper in residentAttributesWrappers.items)
            {
                if (
                    (Registry.appState.Tools.inspectToolState.inspectedEntity is Resident) &&
                    residentAttributesWrapper.resident == (Registry.appState.Tools.inspectToolState.inspectedEntity as Resident)
                )
                {
                    SetText();
                }
            }
        }
    }
}