using System;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class ToolsPanelManger : MonoBehaviour
    {
        ToolStateButtonsManager toolStateButtonsManager;
        BuildToolStateButtonsManager buildToolStateButtonsManager;

        Text descriptionText;

        void Awake()
        {
            toolStateButtonsManager = transform.Find("ToolStateButtons").GetComponent<ToolStateButtonsManager>();
            buildToolStateButtonsManager = transform.Find("BuildToolStateButtons").GetComponent<BuildToolStateButtonsManager>();

            descriptionText = transform.Find("DescriptionText").GetComponent<Text>();

            ToggleBuildStateButtonsPanel(false);
            UpdateDescriptionText();

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.Build.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        void OnToolStateUpdated(ApplicationState.Tools.State.Key toolState, ApplicationState.Tools.State.Key previousToolState)
        {
            UpdateDescriptionText();

            switch (toolState)
            {
                case ApplicationState.Tools.State.Key.Build:
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
            ApplicationState.Tools.State.Key toolState = Registry.appState.Tools.currentKey;

            if (toolState == ApplicationState.Tools.State.Key.Build)
            {
                EntityDefinition selectedEntityDefinition = Registry.appState.Tools.Build.selectedEntityDefinition;

                if (selectedEntityDefinition == null)
                {
                    descriptionText.text = $"{toolState}";
                }
                else
                {
                    Type selectedEntityType = Registry.appState.Tools.Build.selectedEntityType;
                    Entity blueprintEntity = Registry.appState.Tools.Build.blueprintEntity;

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
            if (show)
            {
                buildToolStateButtonsManager.Open();
            }
            else
            {
                buildToolStateButtonsManager.Close();
            }
        }
    }
}