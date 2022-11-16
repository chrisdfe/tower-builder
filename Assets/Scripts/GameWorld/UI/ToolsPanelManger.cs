using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
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

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.events.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
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

        void OnSelectedRoomTemplateUpdated(RoomTemplate selectedRoomTemplate)
        {
            UpdateDescriptionText();
        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.appState.Tools.toolState;

            if (toolState == ToolState.Build)
            {
                RoomTemplate selectedRoomTemplate = Registry.appState.Tools.buildToolState.subStates.roomEntityType.selectedRoomTemplate;
                if (selectedRoomTemplate == null)
                {
                    descriptionText.text = $"{toolState}";
                }
                else
                {
                    Room blueprintRoom = Registry.appState.Tools.buildToolState.subStates.roomEntityType.blueprintRoom;
                    if (blueprintRoom != null)
                    {
                        int price = blueprintRoom.price;
                        descriptionText.text = $"{toolState} - {selectedRoomTemplate.title}: ${price}";
                    }
                    else
                    {
                        descriptionText.text = $"{toolState}";
                    }
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