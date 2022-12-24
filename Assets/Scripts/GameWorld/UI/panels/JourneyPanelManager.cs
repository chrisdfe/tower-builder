using System;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Journeys;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class JourneyPanelManager : MonoBehaviour
    {
        Text descriptionText;

        void Awake()
        {
            descriptionText = TransformUtils.FindDeepChild(transform, "Text").GetComponent<Text>();

            Registry.appState.Journeys.events.onJourneyProgressUpdated += OnJourneyProgressUpdated;
        }

        /*
            Internals
        */

        void UpdatePanelText()
        {
            Journey journey = Registry.appState.Journeys.currentJourney;

            string text = "Journey";
            text += $"   total distance: {journey.totalDistance}";
            text += $"   current progress: {journey.currentProgress}";

            descriptionText.text = text;
        }

        /* 
            Event Handlers
        */
        void OnJourneyProgressUpdated(Journey journey)
        {
            UpdatePanelText();
        }
    }
}