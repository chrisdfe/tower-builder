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

            MapUIStore.Events.onToolStateUpdated += OnToolStateUpdated;
            MapUIStore.Events.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1) && Registry.Stores.mapUIStore.state.toolState != ToolState.None)
            {
                TowerBuilder.Stores.MapUI.MapUIStore.Mutations.SetToolState(ToolState.None);
            }
        }

        void OnToolStateUpdated(MapUIStore.Events.StateEventPayload payload)
        {
            UpdateDescriptionText();

            if (payload.state.toolState == ToolState.Build)
            {
                ToggleRoomBlueprintButtonsPanel(true);
            }
            else if (payload.previousState.toolState == ToolState.Build)
            {
                ToggleRoomBlueprintButtonsPanel(false);
            }
        }

        void OnSelectedRoomKeyUpdated(MapUIStore.Events.StateEventPayload payload)
        {
            UpdateDescriptionText();

        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.Stores.mapUIStore.state.toolState;
            if (toolState == ToolState.Build)
            {
                RoomKey selectedRoomKey = Registry.Stores.mapUIStore.state.selectedRoomKey;
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