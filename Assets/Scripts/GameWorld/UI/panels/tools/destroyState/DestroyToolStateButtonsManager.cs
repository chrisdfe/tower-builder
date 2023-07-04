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
    public class DestroyToolStateButtonsManager : MonoBehaviour
    {
        public DestroyModeSelectButtonsRow destroyModeSelectButtonsRow;

        public void Awake() { }

        public void Start()
        {
            Setup();
        }

        public void Setup()
        {
            destroyModeSelectButtonsRow.Setup();
        }

        public void Teardown()
        {
            destroyModeSelectButtonsRow.Teardown();
        }

        public void Open()
        {
            gameObject.SetActive(true);

            destroyModeSelectButtonsRow.HighlightSelectedButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
