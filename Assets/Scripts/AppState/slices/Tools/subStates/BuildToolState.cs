using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public struct Input
        {
            public string selectedEntityCategory;
            public RoomDefinition selectedRoomDefinition;
        }

        public class Events
        {
            public delegate void SelectedEntityKeyEvent(Entity.Type entityKey, Entity.Type previousEntityType);
            public SelectedEntityKeyEvent onSelectedEntityKeyUpdated;

            public delegate void buildIsActiveEvent();
            public buildIsActiveEvent onBuildStart;
            public buildIsActiveEvent onBuildEnd;

            public delegate void SelectedEntityCategoryEvent(string selectedEntityCategory);
            public SelectedEntityCategoryEvent onSelectedEntityCategoryUpdated;

            public delegate void SelectedEntityDefinitionEvent(EntityDefinition selectedEntityDefinition);
            public SelectedEntityDefinitionEvent onSelectedEntityDefinitionUpdated;

            public delegate void blueprintUpdateEvent(Entity blueprintEntity);
            public blueprintUpdateEvent onBlueprintEntityUpdated;
        }

        public Entity.Type selectedEntityType { get; private set; } = Entity.Type.Room;
        public string selectedEntityCategory { get; private set; } = "";
        public EntityDefinition selectedEntityDefinition { get; private set; } = null;
        public Entity blueprintEntity { get; private set; } = null;

        public Events events;

        public bool isLocked = false;
        bool buildIsActive = false;

        // static Dictionary<Entity.Type, StateSlice> entityStateSliceMap = new Dictionary<Entity.Type, StateSlice>();

        public BuildToolState(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
            events = new Events();

            ResetCategoryAndDefinition();

            // entityStateSliceMap = new Dictionary<Entity.Type, StateSlice>() {
            //     { Entity.Type.Room, () => appState.Entities.Rooms },
            //     { Entity.Type.Furniture, () => appState.Entities.Furnitures },
            //     { Entity.Type.Resident, appState.Entities.Residents },
            //     { Entity.Type.TransportationItem, appState.Entities.TransportationItems }
            // };
        }

        public override void Setup()
        {
            base.Setup();
            ResetCategoryAndDefinition();
            CreateBlueprintEntity();
        }

        public override void Teardown()
        {
            base.Teardown();
            RemoveBlueprintEntity();
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartBuild();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndBuild();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            if (isLocked) return;

            ResetBlueprintEntity();
            blueprintEntity.validator.Validate(Registry.appState);

            events.onBlueprintEntityUpdated?.Invoke(blueprintEntity);
        }

        public void SetSelectedEntityKey(Entity.Type entityType)
        {
            this.isLocked = true;
            Entity.Type previousEntityType = this.selectedEntityType;
            this.selectedEntityType = entityType;

            ResetCategoryAndDefinition();
            ResetBlueprintEntity();

            events.onSelectedEntityKeyUpdated?.Invoke(this.selectedEntityType, previousEntityType);
            this.isLocked = false;
        }

        public void SetSelectedEntityCategory(string entityCategory)
        {
            this.selectedEntityCategory = entityCategory;

            this.selectedEntityDefinition = selectedEntityType switch
            {
                Entity.Type.Room =>
                    Registry.Definitions.Entities.Rooms.Queries.FindFirstInCategory(selectedEntityCategory),
                Entity.Type.Furniture =>
                    Registry.Definitions.Entities.Furnitures.Queries.FindFirstInCategory(selectedEntityCategory),
                Entity.Type.Resident =>
                    Registry.Definitions.Entities.Residents.Queries.FindFirstInCategory(selectedEntityCategory),
                Entity.Type.TransportationItem =>
                    Registry.Definitions.Entities.TransportationItems.Queries.FindFirstInCategory(selectedEntityCategory),
                _ => null
            };

            ResetBlueprintEntity();

            events.onSelectedEntityCategoryUpdated?.Invoke(entityCategory);
            events.onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }


        // TODO - this should probably use key instead of title string
        public void SetSelectedEntityDefinition(string keyLabel)
        {
            Debug.Log("SetSelectedEntityDefinition: ");

            switch (selectedEntityType)
            {
                case Entity.Type.Room:
                    Room.Key roomKey = Room.KeyLabelMap.KeyFromValue(keyLabel);
                    this.selectedEntityDefinition = Registry.Definitions.Entities.Rooms.Queries.FindByKey(roomKey);
                    break;
                case Entity.Type.Furniture:
                    Furniture.Key furnitureKey = Furniture.KeyLabelMap.KeyFromValue(keyLabel);
                    this.selectedEntityDefinition = Registry.Definitions.Entities.Furnitures.Queries.FindByKey(furnitureKey);
                    break;
                case Entity.Type.Resident:
                    Resident.Key residentKey = Resident.KeyLabelMap.KeyFromValue(keyLabel);
                    this.selectedEntityDefinition = Registry.Definitions.Entities.Residents.Queries.FindByKey(residentKey);
                    break;
                case Entity.Type.TransportationItem:
                    TransportationItem.Key transportationItemKey = TransportationItem.KeyLabelMap.KeyFromValue(keyLabel);
                    this.selectedEntityDefinition = Registry.Definitions.Entities.TransportationItems.Queries.FindByKey(transportationItemKey);
                    break;
            }

            /*
            this.selectedEntityDefinition = selectedEntityType switch
            {
                Entity.Type.Room => Registry.Definitions.Entities.Rooms.Queries.FindByTitle(title),
                Entity.Type.Furniture => Registry.Definitions.Entities.Furnitures.Queries.FindByTitle(title),
                Entity.Type.Resident => Registry.Definitions.Entities.Residents.Queries.FindByTitle(title),
                Entity.Type.TransportationItem => Registry.Definitions.Entities.TransportationItems.Queries.FindByTitle(title),
                _ => null
            };
            */

            Debug.Log("this.selectedEntityDefinition: ");
            Debug.Log(this.selectedEntityDefinition);

            ResetBlueprintEntity();

            events.onSelectedEntityDefinitionUpdated?.Invoke(selectedEntityDefinition);
        }

        /*
            Internals
        */
        void ResetCategoryAndDefinition()
        {
            switch (selectedEntityType)
            {
                case Entity.Type.Room:
                    selectedEntityCategory = Registry.Definitions.Entities.Rooms.Queries.FindFirstCategory();
                    selectedEntityDefinition = Registry.Definitions.Entities.Rooms.Queries.FindFirstInCategory(selectedEntityCategory);
                    break;
                case Entity.Type.Furniture:
                    selectedEntityCategory = Registry.Definitions.Entities.Furnitures.Queries.FindFirstCategory();
                    selectedEntityDefinition = Registry.Definitions.Entities.Furnitures.Queries.FindFirstInCategory(selectedEntityCategory);
                    break;
                case Entity.Type.Resident:
                    selectedEntityCategory = Registry.Definitions.Entities.Residents.Queries.FindFirstCategory();
                    selectedEntityDefinition = Registry.Definitions.Entities.Residents.Queries.FindFirstInCategory(selectedEntityCategory);
                    break;
                case Entity.Type.TransportationItem:
                    selectedEntityCategory = Registry.Definitions.Entities.TransportationItems.Queries.FindFirstCategory();
                    selectedEntityDefinition = Registry.Definitions.Entities.TransportationItems.Queries.FindFirstInCategory(selectedEntityCategory);
                    break;
            };
        }

        void StartBuild()
        {
            if (isLocked) return;
            buildIsActive = true;

            events.onBuildStart?.Invoke();
        }

        void EndBuild()
        {
            // EndBuild can be called when the mouse click up event happens outside the screen or over a UI element
            if (isLocked || !buildIsActive) return;
            buildIsActive = false;

            blueprintEntity.validator.Validate(Registry.appState);

            if (blueprintEntity.validator.isValid)
            {
                BuildBlueprintEntity();
                CreateBlueprintEntity();
            }
            else
            {
                Registry.appState.Notifications.Add(
                    new NotificationsList(
                        blueprintEntity.validator.errors.items
                            .Select(error => new Notification(error.message))
                            .ToList()
                    )
                );

                ResetBlueprintEntity();
            }

            events.onBuildEnd?.Invoke();
        }

        void CreateBlueprintEntity()
        {
            Type type = typeof(Room);
            // Room room = Activator.CreateInstance(type);

            blueprintEntity = selectedEntityType switch
            {
                Entity.Type.Room => new Room(selectedEntityDefinition as RoomDefinition),
                Entity.Type.Furniture => new Furniture(selectedEntityDefinition as FurnitureDefinition),
                Entity.Type.Resident => new Resident(selectedEntityDefinition as ResidentDefinition),
                Entity.Type.TransportationItem => new TransportationItem(selectedEntityDefinition as TransportationItemDefinition),
                _ => null
            };

            blueprintEntity.isInBlueprintMode = true;
            blueprintEntity.CalculateCellsFromSelectionBox(Registry.appState.UI.selectionBox);
            blueprintEntity.validator.Validate(Registry.appState);

            switch (selectedEntityType)
            {
                case Entity.Type.Room:
                    Registry.appState.Entities.Rooms.Add(blueprintEntity as Room);
                    break;
                case Entity.Type.Resident:
                    Registry.appState.Entities.Residents.Add(blueprintEntity as Resident);
                    break;
                case Entity.Type.Furniture:
                    Registry.appState.Entities.Furnitures.Add(blueprintEntity as Furniture);
                    break;
                case Entity.Type.TransportationItem:
                    Registry.appState.Entities.TransportationItems.Add(blueprintEntity as TransportationItem);
                    break;
            }
        }

        void BuildBlueprintEntity()
        {
            switch (selectedEntityType)
            {
                case Entity.Type.Room:
                    Registry.appState.Entities.Rooms.Build(blueprintEntity as Room);
                    break;
                case Entity.Type.Resident:
                    Registry.appState.Entities.Residents.Build(blueprintEntity as Resident);
                    break;
                case Entity.Type.Furniture:
                    Registry.appState.Entities.Furnitures.Build(blueprintEntity as Furniture);
                    break;
                case Entity.Type.TransportationItem:
                    Registry.appState.Entities.TransportationItems.Build(blueprintEntity as TransportationItem);
                    break;
            }

            blueprintEntity = null;
        }

        void RemoveBlueprintEntity()
        {
            if (blueprintEntity == null) return;

            switch (blueprintEntity)
            {
                case Room roomBlueprintEntity:
                    Registry.appState.Entities.Rooms.Remove(roomBlueprintEntity);
                    break;
                case Resident residentBlueprintEntity:
                    Registry.appState.Entities.Residents.Remove(residentBlueprintEntity);
                    break;
                case Furniture furnitureBlueprintEntity:
                    Registry.appState.Entities.Furnitures.Remove(furnitureBlueprintEntity);
                    break;
                case TransportationItem transportationItemEntity:
                    Registry.appState.Entities.TransportationItems.Remove(transportationItemEntity);
                    break;
            }

            blueprintEntity = null;
        }

        void ResetBlueprintEntity()
        {
            RemoveBlueprintEntity();
            CreateBlueprintEntity();
        }
    }
}