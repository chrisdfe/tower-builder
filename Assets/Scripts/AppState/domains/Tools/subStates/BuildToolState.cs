using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.State.Tools
{
    public class BuildToolState : ToolStateBase
    {
        public struct Input
        {
            public CellCoordinates buildStartCell;
            public string selectedRoomCategory;
            public RoomTemplate selectedRoomTemplate;
            public bool? buildIsActive;
        }

        public class Events
        {
            public UI.State.CellCoordinatesEvent onBuildStartCellUpdated;

            public delegate void SelectedRoomCategoryEvent(string selectedRoomCategory);
            public SelectedRoomCategoryEvent onSelectedRoomCategoryUpdated;

            public delegate void SelectedRoomTemplateEvent(RoomTemplate selectedRoomTemplate);
            public SelectedRoomTemplateEvent onSelectedRoomTemplateUpdated;

            public delegate void buildIsActiveEvent();
            public buildIsActiveEvent onBuildStart;
            public buildIsActiveEvent onBuildEnd;

            public delegate void blueprintUpdateEvent(Room blueprintRoom);
            public blueprintUpdateEvent onBlueprintRoomUpdated;
        }

        public CellCoordinates buildStartCell { get; private set; } = null;
        public string selectedRoomCategory { get; private set; }
        public RoomTemplate selectedRoomTemplate { get; private set; }
        public bool buildIsActive { get; private set; } = false;

        public Room blueprintRoom { get; private set; } = null;

        public CellCoordinates buildStartCoordinates { get; private set; } = new CellCoordinates(0, 0);
        public CellCoordinates buildEndCoordinates { get; private set; } = new CellCoordinates(0, 0);

        public BuildToolState.Events events;

        // TODO - use this property as a cache and/or have a GetSelectionBox method
        SelectionBox selectionBox
        {
            get
            {
                return new SelectionBox(buildStartCoordinates, buildEndCoordinates);
            }
        }

        public BuildToolState(Tools.State state, Input input) : base(state)
        {
            buildStartCell = input.buildStartCell ?? null;
            selectedRoomCategory = input.selectedRoomCategory ?? "";
            selectedRoomTemplate = input.selectedRoomTemplate ?? Registry.roomTemplates.roomTemplates[0];
            buildIsActive = input.buildIsActive ?? false;

            events = new BuildToolState.Events();
        }

        public override void Setup()
        {
            base.Setup();
            CreateBlueprintRoom();

            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            buildStartCoordinates = currentSelectedCell;
            buildEndCoordinates = currentSelectedCell;
            buildIsActive = false;
        }

        public override void Teardown()
        {
            base.Teardown();
            DestroyBlueprintRoom();

            Registry.appState.UI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;

            buildStartCell = null;
            buildIsActive = false;
        }

        public override void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            if (buildIsActive)
            {
                buildEndCoordinates = currentSelectedCell;
            }
            else
            {
                buildStartCoordinates = currentSelectedCell;
                buildEndCoordinates = currentSelectedCell;
            }

            ResetBlueprintRoom();
            blueprintRoom.validator.Validate(Registry.appState);

            if (events.onBlueprintRoomUpdated != null)
            {
                events.onBlueprintRoomUpdated(blueprintRoom);
            }
        }

        public void SetSelectedRoomCategory(string roomCategory)
        {
            selectedRoomCategory = roomCategory;
            List<RoomTemplate> roomTemplates = Registry.roomTemplates.FindByCategory(selectedRoomCategory);

            RoomTemplate roomTemplate = roomTemplates[0];

            if (roomTemplate != null)
            {
                SelectRoomTemplateAndUpdateBlueprint(roomTemplate);
            }

            if (events.onSelectedRoomCategoryUpdated != null)
            {
                events.onSelectedRoomCategoryUpdated(roomCategory);
            }

            if (roomTemplate != null && events.onSelectedRoomTemplateUpdated != null)
            {
                events.onSelectedRoomTemplateUpdated(selectedRoomTemplate);
            }
        }

        public void SetSelectedRoomTemplate(RoomTemplate roomTemplate)
        {
            SelectRoomTemplateAndUpdateBlueprint(roomTemplate);

            if (events.onSelectedRoomTemplateUpdated != null)
            {
                events.onSelectedRoomTemplateUpdated(selectedRoomTemplate);
            }
        }

        public void StartBuild()
        {
            buildIsActive = true;

            buildStartCoordinates = Registry.appState.UI.currentSelectedCell.Clone();
            buildEndCoordinates = Registry.appState.UI.currentSelectedCell.Clone();

            if (events.onBuildStart != null)
            {
                events.onBuildStart();
            }
        }

        public void EndBuild()
        {
            buildIsActive = false;

            blueprintRoom.validator.Validate(Registry.appState);

            if (blueprintRoom.validator.isValid)
            {
                BuildBlueprintRoom();

                ResetBuildCoordinates();
                CreateBlueprintRoom();
            }
            else
            {
                Registry.appState.Notifications.AddNotifications(
                    blueprintRoom.validator.errors.Select(error => error.message).ToArray()
                );

                ResetBuildCoordinates();
                ResetBlueprintRoom();
            }

            if (events.onBuildEnd != null)
            {
                events.onBuildEnd();
            }
        }

        void SelectRoomTemplateAndUpdateBlueprint(RoomTemplate roomTemplate)
        {
            this.selectedRoomTemplate = roomTemplate;
            ResetBlueprintRoom();
        }

        void CreateBlueprintRoom()
        {
            blueprintRoom = new Room(selectedRoomTemplate);
            blueprintRoom.isInBlueprintMode = true;
            SetBlueprintRoomCells();
            blueprintRoom.validator.Validate(Registry.appState);

            Registry.appState.Rooms.AddRoom(blueprintRoom);
        }

        void BuildBlueprintRoom()
        {
            Registry.appState.Rooms.BuildRoom(blueprintRoom);
            blueprintRoom = null;
        }

        void DestroyBlueprintRoom()
        {
            Registry.appState.Rooms.DestroyRoom(blueprintRoom);
            blueprintRoom = null;
        }

        void ResetBlueprintRoom()
        {
            DestroyBlueprintRoom();
            CreateBlueprintRoom();
        }

        void ResetBuildCoordinates()
        {
            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            buildStartCoordinates = currentSelectedCell;
            buildEndCoordinates = currentSelectedCell;
        }

        void SetBlueprintRoomCells()
        {
            CellCoordinates blockCount = new CellCoordinates(1, 1);

            if (blueprintRoom.resizability.Matches(RoomResizability.Inflexible()))
            {
                blueprintRoom.bottomLeftCoordinates = buildStartCoordinates;
            }
            else
            {
                // Restrict resizability to X/floor depending on RoomFlexibility
                if (blueprintRoom.resizability.x)
                {
                    blueprintRoom.bottomLeftCoordinates = new CellCoordinates(
                        selectionBox.bottomLeft.x,
                        buildStartCoordinates.floor
                    );
                    blockCount.x = MathUtils.RoundUpToNearest(selectionBox.dimensions.width, blueprintRoom.blockDimensions.width);
                }

                if (blueprintRoom.resizability.floor)
                {
                    blueprintRoom.bottomLeftCoordinates = new CellCoordinates(
                        buildStartCoordinates.x,
                        selectionBox.bottomLeft.floor
                    );
                    blockCount.floor = MathUtils.RoundUpToNearest(selectionBox.dimensions.height, blueprintRoom.blockDimensions.height);
                }
            }

            blueprintRoom.CalculateRoomCells(blockCount);
        }
    }
}