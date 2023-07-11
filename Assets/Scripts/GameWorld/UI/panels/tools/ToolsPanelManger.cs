using System;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class ToolsPanelManger : MonoBehaviour
    {
        ToolStateButtonsRow toolStateButtonsRow;
        BuildToolStateButtonsManager buildToolStateButtonsManager;
        DestroyToolStateButtonsManager destroyToolStateButtonsManager;

        Text descriptionText;

        void Awake()
        {
            toolStateButtonsRow = transform.Find("ToolStateButtons").GetComponent<ToolStateButtonsRow>();
            buildToolStateButtonsManager = transform.Find("BuildToolStateButtons").GetComponent<BuildToolStateButtonsManager>();
            destroyToolStateButtonsManager = transform.Find("DestroyToolStateButtons").GetComponent<DestroyToolStateButtonsManager>();

            descriptionText = transform.Find("DescriptionText").GetComponent<Text>();

            buildToolStateButtonsManager.Close();
            destroyToolStateButtonsManager.Close();
            UpdateDescriptionText();

            Registry.appState.Tools.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.Build.Entities.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        void OnToolStateUpdated(ApplicationState.Tools.State.Key toolState, ApplicationState.Tools.State.Key previousToolState)
        {
            UpdateDescriptionText();

            switch (toolState)
            {
                case ApplicationState.Tools.State.Key.Build:
                    buildToolStateButtonsManager.Open();
                    destroyToolStateButtonsManager.Close();
                    break;
                case ApplicationState.Tools.State.Key.Destroy:
                    buildToolStateButtonsManager.Close();
                    destroyToolStateButtonsManager.Open();
                    break;
                default:
                    buildToolStateButtonsManager.Close();
                    destroyToolStateButtonsManager.Close();
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
                EntityDefinition selectedEntityDefinition = Registry.appState.Tools.Build.Entities.selectedEntityDefinition;

                if (selectedEntityDefinition == null)
                {
                    descriptionText.text = $"{toolState}";
                }
                else
                {
                    Type selectedEntityType = Registry.appState.Tools.Build.Entities.selectedEntityType;
                    EntityGroup blueprint = Registry.appState.Tools.Build.blueprint;

                    if (blueprint != null)
                    {
                        int price = blueprint.price;
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
    }
}