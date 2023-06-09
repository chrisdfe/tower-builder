using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class RoomModeButtonsManager : MonoBehaviour, ISetupable
    {
        public RoomDefinitionButtonsRow roomDefinitionButtonsRow;

        void Awake()
        {

        }

        public void Open()
        {
            gameObject.SetActive(true);

            roomDefinitionButtonsRow.HighlightSelectedButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Setup()
        {
            roomDefinitionButtonsRow.Setup();
        }

        public void Teardown()
        {
            roomDefinitionButtonsRow.Teardown();
        }
    }
}