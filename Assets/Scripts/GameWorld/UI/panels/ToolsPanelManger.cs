using System;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class ToolsPanelManger : MonoBehaviour
    {
        ToolStateButtonsManager toolStateButtonsManager;
        BuildStateButtonsManager buildStateButtonsManager;

        Text descriptionText;

        void Awake()
        {
            toolStateButtonsManager = transform.Find("ToolStateButtons").GetComponent<ToolStateButtonsManager>();
            buildStateButtonsManager = transform.Find("BuildStateButtons").GetComponent<BuildStateButtonsManager>();

            descriptionText = transform.Find("DescriptionText").GetComponent<Text>();

            ToggleBuildStateButtonsPanel(false);
            UpdateDescriptionText();

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        void OnToolStateUpdated(ToolState toolState, ToolState previousToolState)
        {
            UpdateDescriptionText();

            switch (toolState)
            {
                case ToolState.Build:
                    ToggleBuildStateButtonsPanel(true);
                    break;
                default:
                    ToggleBuildStateButtonsPanel(false);
                    break;
            }
        }

        void OnSelectedEntityDefinitionUpdated(EntityDefinition selectedEntityDefinition)
        {
            UpdateDescriptionText();
        }

        void UpdateDescriptionText()
        {
            ToolState toolState = Registry.appState.Tools.toolState;

            if (toolState == ToolState.Build)
            {
                EntityDefinition selectedEntityDefinition = Registry.appState.Tools.buildToolState.selectedEntityDefinition;

                if (selectedEntityDefinition == null)
                {
                    descriptionText.text = $"{toolState}";
                }
                else
                {
                    Type selectedEntityType = Registry.appState.Tools.buildToolState.selectedEntityType;
                    Entity blueprintEntity = Registry.appState.Tools.buildToolState.blueprintEntity;

                    if (blueprintEntity != null)
                    {
                        int price = blueprintEntity.price;
                        descriptionText.text = $"{toolState} - {selectedEntityType} -{selectedEntityDefinition.title}: ${price}";
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
    }
}