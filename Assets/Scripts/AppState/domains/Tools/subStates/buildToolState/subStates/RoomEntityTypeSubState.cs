using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.Utils;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class RoomEntityTypeSubState : EntityTypeSubState
        {
            public string selectedRoomCategory { get; private set; } = "";
            public RoomTemplate selectedRoomTemplate { get; private set; } = null;
            public Room blueprintRoom { get; private set; } = null;

            public class Events
            {
                public delegate void SelectedRoomCategoryEvent(string selectedRoomCategory);
                public SelectedRoomCategoryEvent onSelectedRoomCategoryUpdated;

                public delegate void SelectedRoomTemplateEvent(RoomTemplate selectedRoomTemplate);
                public SelectedRoomTemplateEvent onSelectedRoomTemplateUpdated;

                public delegate void blueprintUpdateEvent(Room blueprintRoom);
                public blueprintUpdateEvent onBlueprintRoomUpdated;
            }

            public Events events;

            public RoomEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState)
            {
                events = new Events();

                selectedRoomTemplate = Registry.definitions.rooms.templates[0];
            }

            public override void Setup()
            {
                base.Setup();

                CreateBlueprintRoom();
            }

            public override void Teardown()
            {
                base.Teardown();

                DestroyBlueprintRoom();
            }

            public override void EndBuild()
            {
                base.EndBuild();
                blueprintRoom.validator.Validate(Registry.appState);

                if (blueprintRoom.validator.isValid)
                {
                    BuildBlueprintRoom();

                    // ResetBuildCoordinates();
                    CreateBlueprintRoom();
                }
                else
                {
                    Registry.appState.Notifications.AddNotifications(
                        blueprintRoom.validator.errors.Select(error => error.message).ToArray()
                    );

                    // ResetBuildCoordinates();
                    ResetBlueprintRoom();
                }
            }

            public override void OnSelectionBoxUpdated()
            {
                base.OnSelectionBoxUpdated();

                ResetBlueprintRoom();
                blueprintRoom.validator.Validate(Registry.appState);

                if (events.onBlueprintRoomUpdated != null)
                {
                    events.onBlueprintRoomUpdated(blueprintRoom);
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

            public void SetSelectedRoomCategory(string roomCategory)
            {
                selectedRoomCategory = roomCategory;
                List<RoomTemplate> roomDefinitions = Registry.definitions.rooms.queries.FindByCategory(selectedRoomCategory);

                RoomTemplate roomTemplate = roomDefinitions[0];

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

            void SetBlueprintRoomCells()
            {
                SelectionBox selectionBox = Registry.appState.UI.selectionBox;
                CellCoordinates blockCount = new CellCoordinates(1, 1);

                if (blueprintRoom.resizability == RoomResizability.Inflexible)
                {
                    blueprintRoom.bottomLeftCoordinates = selectionBox.start;
                }
                else
                {
                    // Restrict resizability to X/floor depending on RoomFlexibility
                    switch (blueprintRoom.resizability)
                    {
                        case RoomResizability.Flexible:
                            CalculateHorizontalBlocks();
                            CalculateVerticalBlocks();
                            break;
                        case RoomResizability.Horizontal:
                            CalculateHorizontalBlocks();
                            break;
                        case RoomResizability.Vertical:
                            CalculateVerticalBlocks();
                            break;
                        case RoomResizability.Inflexible:
                            break;
                    }

                    void CalculateHorizontalBlocks()
                    {
                        blueprintRoom.bottomLeftCoordinates = new CellCoordinates(
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.x,
                            selectionBox.start.floor
                        );
                        blockCount.x = MathUtils.RoundUpToNearest(selectionBox.dimensions.width, blueprintRoom.blockDimensions.width);
                    }

                    void CalculateVerticalBlocks()
                    {
                        blueprintRoom.bottomLeftCoordinates = new CellCoordinates(
                            selectionBox.start.x,
                            selectionBox.cellCoordinatesList.bottomLeftCoordinates.floor
                        );
                        blockCount.floor = MathUtils.RoundUpToNearest(selectionBox.dimensions.height, blueprintRoom.blockDimensions.height);
                    }
                }

                blueprintRoom.CalculateRoomCells(blockCount);
            }
        }
    }
}
