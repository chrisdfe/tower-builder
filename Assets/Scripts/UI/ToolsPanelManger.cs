using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class ToolsPanelManger : MonoBehaviour
    {
        ToolStateButtonsManager toolStateButtonsManager;
        RoomBlueprintButtonsManager roomBlueprintButtonsManager;

        Text descriptionText;

        void Awake()
        {
            toolStateButtonsManager = transform.Find("ToolStateButtons").GetComponent<ToolStateButtonsManager>();
            roomBlueprintButtonsManager = transform.Find("RoomBlueprintButtons").GetComponent<RoomBlueprintButtonsManager>();

            descriptionText = transform.Find("DescriptionText").GetComponent<Text>();

            ToggleRoomBlueprintButtonsPanel(false);
            UpdateDescriptionText();

            Registry.Stores.MapUI.onToolStateUpdated += OnToolStateUpdated;
            Registry.Stores.MapUI.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1) && Registry.Stores.MapUI.toolState != ToolState.None)
            {
                Registry.Stores.MapUI.SetToolState(ToolState.None);
            }
        }

        void OnToolStateUpdated(ToolState toolState, ToolState previousToolState)
        {
            UpdateDescriptionText();

            if (toolState == ToolState.Build)
            {
                ToggleRoomBlueprintButtonsPanel(true);
            }
            else if (previousToolState == ToolState.Build)
            {
                ToggleRoomBlueprintButtonsPanel(false);
            }
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            UpdateDescriptionText();

        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.Stores.MapUI.toolState;
            if (toolState == ToolState.Build)
            {
                RoomKey selectedRoomKey = Registry.Stores.MapUI.selectedRoomKey;
                descriptionText.text = $"{toolState} - {selectedRoomKey}";
            }
            else
            {
                descriptionText.text = $"{toolState}";
            }
        }

        void ToggleRoomBlueprintButtonsPanel(bool show)
        {
            roomBlueprintButtonsManager.gameObject.SetActive(show);
        }
    }
}