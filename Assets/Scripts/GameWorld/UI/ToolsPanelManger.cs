using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Blueprints;
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
            Registry.Stores.UI.buildToolSubState.onSelectedRoomDetailsUpdated += OnSelectedRoomDetailsUpdated;
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

        void OnSelectedRoomDetailsUpdated(RoomDetails selectedRoomDetails)
        {
            UpdateDescriptionText();
        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.Stores.UI.toolState;
            if (toolState == ToolState.Build)
            {
                RoomDetails selectedRoomDetails = Registry.Stores.UI.buildToolSubState.selectedRoomDetails;
                if (selectedRoomDetails == null)
                {
                    descriptionText.text = $"{toolState}";
                }
                else
                {
                    Blueprint blueprint = Registry.Stores.UI.buildToolSubState.currentBlueprint;
                    int price = blueprint.room.GetPrice();
                    descriptionText.text = $"{toolState} - {selectedRoomDetails.title}: ${price}";
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