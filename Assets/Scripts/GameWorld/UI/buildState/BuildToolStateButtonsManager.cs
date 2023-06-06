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
        public EntityTypeButtonsRow entityTypeButtonsRow;
        public CategoryButtonsRow categoryButtonsRow;
        public DefinitionButtonsRow definitionButtonsRow;

        void Awake()
        {
            Registry.appState.Tools.buildToolState.events.onSelectedEntityKeyUpdated += OnSelectedEntityKeyUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityCategoryUpdated += OnSelectedEntityCategoryUpdated;
            Registry.appState.Tools.buildToolState.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        void Start()
        {
            Setup();
        }

        public void Open()
        {
            gameObject.SetActive(true);

            entityTypeButtonsRow.HighlightSelectedButton();
            categoryButtonsRow.HighlightSelectedButton();
            definitionButtonsRow.HighlightSelectedButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Setup()
        {
            entityTypeButtonsRow.Setup();
            categoryButtonsRow.Setup();
            definitionButtonsRow.Setup();
        }

        public void Teardown()
        {
            entityTypeButtonsRow.Teardown();
            categoryButtonsRow.Teardown();
            definitionButtonsRow.Teardown();
        }


        void OnSelectedEntityKeyUpdated(Type entityType, Type previousEntityType)
        {
            if (entityType == previousEntityType) return;

            entityTypeButtonsRow.HighlightSelectedButton();
            categoryButtonsRow.Reset();
            definitionButtonsRow.Reset();
        }

        void OnSelectedEntityCategoryUpdated(string newEntityCategory)
        {
            categoryButtonsRow.HighlightSelectedButton();
            definitionButtonsRow.Reset();
        }

        void OnSelectedEntityDefinitionUpdated(EntityDefinition entityDefinition)
        {
            definitionButtonsRow.HighlightSelectedButton();
        }
    }
}
