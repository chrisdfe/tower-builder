using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.GameWorld.UI.Components;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildToolStateButtonsManager : MonoBehaviour
    {

        public EntitiesModeButtonsManager entityModeButtonsManager;
        public RoomModeButtonsManager roomModeButtonsManager;

        public BuildModeSelectButtonsRow buildModeButtonsRow;

        void Awake()
        {

        }

        void Start()
        {
            Setup();
        }

        public void Setup()
        {
            buildModeButtonsRow.Setup();
            entityModeButtonsManager.Setup();
            roomModeButtonsManager.Setup();

            Registry.appState.Tools.Build.onModeUpdated += OnBuildModeUpdated;
        }

        public void Teardown()
        {
            buildModeButtonsRow.Teardown();
            entityModeButtonsManager.Setup();
            roomModeButtonsManager.Setup();

            Registry.appState.Tools.Build.onModeUpdated -= OnBuildModeUpdated;
        }

        public void Open()
        {
            gameObject.SetActive(true);

            buildModeButtonsRow.HighlightSelectedButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        void OnBuildModeUpdated(ApplicationState.Tools.Build.State.Mode newMode, ApplicationState.Tools.Build.State.Mode previousMode)
        {
            switch (newMode)
            {
                case ApplicationState.Tools.Build.State.Mode.Entities:
                    entityModeButtonsManager.Open();
                    roomModeButtonsManager.Close();
                    break;
                case ApplicationState.Tools.Build.State.Mode.Rooms:
                    entityModeButtonsManager.Close();
                    roomModeButtonsManager.Open();
                    break;
            }
        }
    }
}
