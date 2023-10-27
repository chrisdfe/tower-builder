using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;

using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
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
        List<string> textBuffer = new List<string>();

        void Awake()
        {
            inspectText = TransformUtils.FindDeepChild(transform, "InspectText").GetComponent<Text>();
            inspectIndexText = TransformUtils.FindDeepChild(transform, "InspectIndexText").GetComponent<Text>();
            actionButtonsWrapper = TransformUtils.FindDeepChild(transform, "ActionButtonsWrapper");

            SetText();

            Setup();
        }

        public void Setup()
        {
            // Registry.appState.Attributes.Residents.onItemsUpdated += OnResidentAttributesUpdated;

            // Registry.appState.Tools.Inspect.onInspectedEntityListUpdated += OnInspectedEntityListUpdated;
            // Registry.appState.Tools.Inspect.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            // Registry.appState.Attributes.Residents.onItemsUpdated -= OnResidentAttributesUpdated;

            // Registry.appState.Tools.Inspect.onInspectedEntityListUpdated -= OnInspectedEntityListUpdated;
            // Registry.appState.Tools.Inspect.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        public void Update()
        {
            SetText();

            inspectText.text = String.Join("\n", textBuffer);
            textBuffer = new List<string>();
        }

        /* 
            Internals
        */
        void SetText()
        {
            // SetInspectIndexText();
            SetInspectText();
            SetActionButtons();
        }

        /*
        void SetInspectIndexText()
        {
            ListWrapper<Entity> inspectedEntityList = Registry.appState.Tools.Inspect.inspectedEntityList;
            int index = Registry.appState.Tools.Inspect.inspectedEntityIndex;
            Entity inspectedEntity = Registry.appState.Tools.Inspect.inspectedEntity;

            string text = $"entityList: {inspectedEntityList.Count}\n"
                + $"index: {index}\n"
                + $"inspectedEntity: {inspectedEntity}";

            inspectIndexText.text = text;
        }
        */

        void SetInspectText()
        {
            Entity targetEntity = null;

            if (Registry.appState.Tools.Inspect.inspectedEntity != null)
            {
                targetEntity = Registry.appState.Tools.Inspect.inspectedEntity;
            }
            else if (Registry.appState.UI.entitiesInSelection.Count > 0)
            {
                targetEntity = Registry.appState.UI.entitiesInSelection[0];
            }

            if (targetEntity == null)
            {
                textBuffer.Add("None");
            }
            else
            {
                textBuffer.Add($"{targetEntity}");
                textBuffer.Add($"parent - {Registry.appState.EntityGroups.FindEntityParent(targetEntity)}");

                switch (targetEntity)
                {
                    case Resident residentEntity:
                        SetInspectedResidentText(residentEntity);
                        break;
                    case TransportationItem transportationItemEntity:
                        SetInspectedTransportationItemText(transportationItemEntity);
                        break;
                }
            }
        }

        void SetInspectedResidentText(Resident resident)
        {
            textBuffer.Add($"name: {resident}");
            textBuffer.Add($"state: {resident.behavior.currentState}");

            textBuffer.Add("attributes:");
            resident.attributes.asTupleList.ForEach(tuple =>
            {
                var (key, attribute) = tuple;
                textBuffer.Add($"{key}: {attribute.value}");
            });
        }

        void SetInspectedTransportationItemText(TransportationItem transportationItem)
        {
            if (transportationItem != null)
            {
                textBuffer.Add($"   name: {transportationItem}");
            }
        }

        void SetActionButtons()
        {
            DestroyActionButtons();

            Entity inspectedEntity = Registry.appState.Tools.Inspect.inspectedEntity;

            switch (inspectedEntity)
            {
                case Furniture furnitureEntity:
                    CreateFurnitureActionButtons(furnitureEntity);
                    break;
                case Resident residentEntity:
                    CreateResidentActionButtons(residentEntity);
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
    }
}