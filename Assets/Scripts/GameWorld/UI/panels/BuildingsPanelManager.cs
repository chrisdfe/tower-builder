using System;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Journeys;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildingsPanelManager : MonoBehaviour
    {
        Text descriptionText;

        void Awake()
        {
            descriptionText = TransformUtils.FindDeepChild(transform, "Text").GetComponent<Text>();
        }

        void Update()
        {
            UpdatePanelText();
        }

        /*
            Internals
        */
        void UpdatePanelText()
        {
            string text = "";
            text += $"# Buildings: {Registry.appState.EntityGroups.Buildings.list.Count}\n";
            text += $"# Rooms: {Registry.appState.EntityGroups.Rooms.list.Count}";

            if (Registry.appState.EntityGroups.Rooms.list.Count > 0)
            {
                text += "\n";
                text += Registry.appState.EntityGroups.Rooms.list.items[0].isInBlueprintMode;
            }

            descriptionText.text = text;
        }
    }
}