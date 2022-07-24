using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.Definitions.Templates;
using UnityEngine;

namespace TowerBuilder.State.UI
{
    public class BuildToolState : ToolStateBase
    {
        public struct Input
        {
            public CellCoordinates buildStartCell;
            public string selectedRoomCategory;
            public RoomTemplate selectedRoomTemplate;
            public bool? buildIsActive;
            public RoomConnections blueprintRoomConnections;
        }

        public CellCoordinates buildStartCell { get; private set; } = null;
        public UI.State.cellCoordinatesEvent onBuildStartCellUpdated;

        public string selectedRoomCategory { get; private set; }
        public delegate void SelectedRoomCategoryEvent(string selectedRoomCategory);
        public SelectedRoomCategoryEvent onSelectedRoomCategoryUpdated;

        public RoomTemplate selectedRoomTemplate { get; private set; }
        public delegate void SelectedRoomTemplateEvent(RoomTemplate selectedRoomTemplate);
        public SelectedRoomTemplateEvent onSelectedRoomTemplateUpdated;

        public bool buildIsActive { get; private set; } = false;

        public delegate void buildIsActiveEvent(bool buildIsActive);
        public buildIsActiveEvent onBuildStart;
        public buildIsActiveEvent onBuildEnd;

        // this roomBlueprint is essentially just derived data, so no events needed
        public Blueprint currentBlueprint { get; private set; }

        public RoomConnections blueprintRoomConnections;
        public delegate void BlueprintRoomConnectionEvent(RoomConnections roomConnections);
        public BlueprintRoomConnectionEvent onBlueprintRoomConnectionsUpdated;

        public BuildToolState(UI.State state, Input input) : base(state)
        {
            buildStartCell = input.buildStartCell ?? null;
            selectedRoomCategory = input.selectedRoomCategory ?? "";
            selectedRoomTemplate = input.selectedRoomTemplate ?? Registry.roomTemplates.roomTemplates[0];
            buildIsActive = input.buildIsActive ?? false;
            blueprintRoomConnections = input.blueprintRoomConnections ?? new RoomConnections();
        }

        public override void Setup()
        {
            CreateBlueprint();
        }

        public override void Teardown()
        {
            DeleteBlueprint();
            ResetBlueprintRoomConnections();

            buildStartCell = null;
            buildIsActive = false;
        }

        public override void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            if (buildIsActive)
            {
                currentBlueprint.SetBuildEndCell(currentSelectedCell);
            }
            else
            {
                currentBlueprint.SetBuildStartCell(currentSelectedCell);
            }

            currentBlueprint.Validate(Registry.appState);
            SearchForBlueprintRoomConnections();
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

            if (onSelectedRoomCategoryUpdated != null)
            {
                onSelectedRoomCategoryUpdated(roomCategory);
            }

            if (roomTemplate != null && onSelectedRoomTemplateUpdated != null)
            {
                onSelectedRoomTemplateUpdated(selectedRoomTemplate);
            }
        }

        public void SetSelectedRoomTemplate(RoomTemplate roomTemplate)
        {
            SelectRoomTemplateAndUpdateBlueprint(roomTemplate);

            if (onSelectedRoomTemplateUpdated != null)
            {
                onSelectedRoomTemplateUpdated(selectedRoomTemplate);
            }
        }

        public void StartBuild()
        {
            buildIsActive = true;

            SetBuildStartCell();

            if (onBuildStart != null)
            {
                onBuildStart(buildIsActive);
            }
        }

        public void EndBuild()
        {
            buildIsActive = false;

            AttemptToCreateRoomFromCurrentBlueprint();

            if (onBuildEnd != null)
            {
                onBuildEnd(buildIsActive);
            }
        }

        public void SetBuildStartCell()
        {
            if (selectedRoomTemplate == null)
            {
                return;
            }

            if (parentState.currentSelectedCell == null)
            {
                return;
            }

            currentBlueprint.SetBuildStartCell(parentState.currentSelectedCell.Clone());

            if (onBuildStartCellUpdated != null)
            {
                onBuildStartCellUpdated(buildStartCell);
            }
        }


        public void SearchForBlueprintRoomConnections()
        {
            if (!currentBlueprint.IsValid())
            {
                ResetBlueprintRoomConnections();
                return;
            }

            RoomConnections newBlueprintConnections =
                blueprintRoomConnections.SearchForNewConnectionsToRoom(Registry.appState.Rooms.roomList, currentBlueprint.room);

            this.blueprintRoomConnections = newBlueprintConnections;

            if (onBlueprintRoomConnectionsUpdated != null)
            {
                onBlueprintRoomConnectionsUpdated(this.blueprintRoomConnections);
            }
        }

        public void ResetBlueprintRoomConnections()
        {
            this.blueprintRoomConnections = new RoomConnections();

            if (onBlueprintRoomConnectionsUpdated != null)
            {
                onBlueprintRoomConnectionsUpdated(this.blueprintRoomConnections);
            }
        }

        void SelectRoomTemplateAndUpdateBlueprint(RoomTemplate roomTemplate)
        {
            this.selectedRoomTemplate = roomTemplate;

            currentBlueprint.SetRoomTemplate(this.selectedRoomTemplate);
            currentBlueprint.Validate(Registry.appState);
        }

        void CreateBlueprint()
        {
            currentBlueprint = new Blueprint(selectedRoomTemplate, parentState.currentSelectedCell);
        }

        void DeleteBlueprint()
        {
            currentBlueprint.OnDestroy();
            currentBlueprint = null;
        }

        void AttemptToCreateRoomFromCurrentBlueprint()
        {
            if (currentBlueprint == null)
            {
                return;
            }

            SearchForBlueprintRoomConnections();
            Registry.appState.Rooms.AttemptToAddRoom(currentBlueprint, blueprintRoomConnections);
            currentBlueprint.Reset();
        }
    }
}