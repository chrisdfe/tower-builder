using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.State.UI;
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

            Registry.appState.UI.toolState.onValueChanged += OnToolStateUpdated;
            Registry.appState.UI.buildToolSubState.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        void Update()
        {
            // Right click to exit out of current state?
            if (Input.GetMouseButtonDown(1) && Registry.appState.UI.toolState.value != ToolState.None)
            {
                Registry.appState.UI.toolState.value = ToolState.None;
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

        void OnSelectedRoomTemplateUpdated(RoomTemplate selectedRoomTemplate)
        {
            UpdateDescriptionText();
        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.appState.UI.toolState.value;
            if (toolState == ToolState.Build)
            {
                RoomTemplate selectedRoomTemplate = Registry.appState.UI.buildToolSubState.selectedRoomTemplate;
                if (selectedRoomTemplate == null)
                {
                    descriptionText.text = $"{toolState}";
                }
                else
                {
                    Blueprint blueprint = Registry.appState.UI.buildToolSubState.currentBlueprint;
                    int price = blueprint.room.price;
                    descriptionText.text = $"{toolState} - {selectedRoomTemplate.title}: ${price}";
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