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
        public GameObject EntityButtons;
        public GameObject EntityGroupButtons;

        public EntityDefinitionButtonsRow entityDefinitionButtonsRow;
        public EntityCategoryButtonsRow entityCategoryButtonsRow;
        public EntityTypeButtonsRow entityTypeButtonsRow;
        public BuildTypeButtonsRow buildTypeButtonsRow;

        void Awake()
        {
            Registry.appState.Tools.Build.events.onSelectedEntityKeyUpdated += OnSelectedEntityKeyUpdated;
            Registry.appState.Tools.Build.events.onSelectedEntityCategoryUpdated += OnSelectedEntityCategoryUpdated;
            Registry.appState.Tools.Build.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        void Start()
        {
            Setup();
        }

        public void Open()
        {
            gameObject.SetActive(true);

            buildTypeButtonsRow.HighlightSelectedButton();
            entityTypeButtonsRow.HighlightSelectedButton();
            entityCategoryButtonsRow.HighlightSelectedButton();
            entityDefinitionButtonsRow.HighlightSelectedButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Setup()
        {
            buildTypeButtonsRow.Setup();
            entityTypeButtonsRow.Setup();
            entityCategoryButtonsRow.Setup();
            entityDefinitionButtonsRow.Setup();
        }

        public void Teardown()
        {
            buildTypeButtonsRow.Teardown();
            entityTypeButtonsRow.Teardown();
            entityCategoryButtonsRow.Teardown();
            entityDefinitionButtonsRow.Teardown();
        }


        void OnSelectedEntityKeyUpdated(Type entityType, Type previousEntityType)
        {
            if (entityType == previousEntityType) return;

            entityTypeButtonsRow.HighlightSelectedButton();
            entityCategoryButtonsRow.Reset();
            entityDefinitionButtonsRow.Reset();
        }

        void OnSelectedEntityCategoryUpdated(string newEntityCategory)
        {
            entityCategoryButtonsRow.HighlightSelectedButton();
            entityDefinitionButtonsRow.Reset();
        }

        void OnSelectedEntityDefinitionUpdated(EntityDefinition entityDefinition)
        {
            entityDefinitionButtonsRow.HighlightSelectedButton();
        }
    }
}
