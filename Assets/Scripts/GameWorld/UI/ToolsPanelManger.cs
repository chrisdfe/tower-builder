using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class ToolsPanelManger : MonoBehaviour
    {
        ToolStateButtonsManager toolStateButtonsManager;
        BuildStateButtonsManager buildStateButtonsManager;
        RoutesStateButtonsManager routesStateButtonsManager;

        Text descriptionText;

        void Awake()
        {
            toolStateButtonsManager = transform.Find("ToolStateButtons").GetComponent<ToolStateButtonsManager>();
            buildStateButtonsManager = transform.Find("BuildStateButtons").GetComponent<BuildStateButtonsManager>();
            routesStateButtonsManager = transform.Find("RoutesStateButtons").GetComponent<RoutesStateButtonsManager>();

            descriptionText = transform.Find("DescriptionText").GetComponent<Text>();

            ToggleBuildStateButtonsPanel(false);
            ToggleRoutesStateButtonsPanel(false);
            UpdateDescriptionText();

            Registry.Stores.UI.onToolStateUpdated += OnToolStateUpdated;
            Registry.Stores.UI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void Update()
        {
            // Right click to exit out of current state?
            if (Input.GetMouseButtonDown(1) && Registry.Stores.UI.toolState != ToolState.None)
            {
                Registry.Stores.UI.SetToolState(ToolState.None);
            }
        }

        void OnToolStateUpdated(ToolState toolState, ToolState previousToolState)
        {
            UpdateDescriptionText();

            switch (toolState)
            {
                case ToolState.Build:
                    ToggleRoutesStateButtonsPanel(false);
                    ToggleBuildStateButtonsPanel(true);
                    break;
                case ToolState.Routes:
                    ToggleRoutesStateButtonsPanel(true);
                    ToggleBuildStateButtonsPanel(false);
                    break;
                default:
                    ToggleRoutesStateButtonsPanel(false);
                    ToggleBuildStateButtonsPanel(false);
                    break;
            }
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            UpdateDescriptionText();
        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.Stores.UI.toolState;
            if (toolState == ToolState.Build)
            {
                RoomKey selectedRoomKey = Registry.Stores.UI.buildToolSubState.selectedRoomKey;
                if (selectedRoomKey == RoomKey.None)
                {
                    descriptionText.text = $"{toolState} - {selectedRoomKey}";
                }
                else
                {
                    RoomDetails roomDetails = Registry.Stores.UI.buildToolSubState.currentBlueprint.room.roomDetails;
                    int price = roomDetails.price;
                    descriptionText.text = $"{toolState} - {selectedRoomKey}: ${price}";
                }
            }
            else
            {
                descriptionText.text = $"{toolState}";
            }
        }

        void ToggleBuildStateButtonsPanel(bool show)
        {
            buildStateButtonsManager.gameObject.SetActive(show);
        }

        void ToggleRoutesStateButtonsPanel(bool show)
        {
            routesStateButtonsManager.gameObject.SetActive(show);
        }
    }
}