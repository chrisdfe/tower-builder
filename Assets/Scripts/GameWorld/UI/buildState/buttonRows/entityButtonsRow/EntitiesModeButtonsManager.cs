using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class EntitiesModeButtonsManager : MonoBehaviour, ISetupable
    {
        public EntityDefinitionButtonsRow entityDefinitionButtonsRow;
        public EntityCategoryButtonsRow entityCategoryButtonsRow;
        public EntityTypeButtonsRow entityTypeButtonsRow;

        void Awake()
        {
            Registry.appState.Tools.Build.Entities.events.onSelectedEntityKeyUpdated += OnSelectedEntityKeyUpdated;
            Registry.appState.Tools.Build.Entities.events.onSelectedEntityCategoryUpdated += OnSelectedEntityCategoryUpdated;
            Registry.appState.Tools.Build.Entities.events.onSelectedEntityDefinitionUpdated += OnSelectedEntityDefinitionUpdated;
        }

        public void Open()
        {
            gameObject.SetActive(true);

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
            entityTypeButtonsRow.Setup();
            entityCategoryButtonsRow.Setup();
            entityDefinitionButtonsRow.Setup();
        }

        public void Teardown()
        {
            entityTypeButtonsRow.Teardown();
            entityCategoryButtonsRow.Teardown();
            entityDefinitionButtonsRow.Teardown();
        }

        /* 
            State Event handlers
        */
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