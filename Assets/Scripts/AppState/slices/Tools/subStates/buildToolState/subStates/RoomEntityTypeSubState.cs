using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.Utils;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class RoomEntityTypeSubState : EntityTypeSubState
        {
            public string selectedRoomCategory { get; private set; } = "";
            public RoomDefinition selectedRoomDefinition { get; private set; } = null;
            public Room blueprintRoom { get; private set; } = null;

            public class Events
            {
                public delegate void SelectedRoomCategoryEvent(string selectedRoomCategory);
                public SelectedRoomCategoryEvent onSelectedRoomCategoryUpdated;

                public delegate void SelectedRoomDefinitionEvent(RoomDefinition selectedRoomDefinition);
                public SelectedRoomDefinitionEvent onSelectedRoomDefinitionUpdated;

                public delegate void blueprintUpdateEvent(Room blueprintRoom);
                public blueprintUpdateEvent onBlueprintRoomUpdated;
            }

            public Events events;

            public RoomEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState)
            {
                events = new Events();

                selectedRoomDefinition = Registry.Definitions.Entities.Rooms.Definitions[0];
            }

            public override void Setup()
            {
                base.Setup();

                CreateBlueprintRoom();
            }

            public override void Teardown()
            {
                base.Teardown();

                RemoveBlueprintRoom();
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
                    Registry.appState.Notifications.Add(
                        new NotificationsList(
                            blueprintRoom.validator.errors.items
                                .Select(error => new Notification(error.message))
                                .ToList()
                        )
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

            void SelectRoomDefinitionAndUpdateBlueprint(RoomDefinition roomDefinition)
            {
                this.selectedRoomDefinition = roomDefinition;
                ResetBlueprintRoom();
            }

            void CreateBlueprintRoom()
            {
                blueprintRoom = new Room(selectedRoomDefinition);
                blueprintRoom.isInBlueprintMode = true;
                blueprintRoom.CalculateCellsFromSelectionBox(Registry.appState.UI.selectionBox);
                blueprintRoom.validator.Validate(Registry.appState);

                Registry.appState.Entities.Rooms.Add(blueprintRoom);
            }

            void BuildBlueprintRoom()
            {
                Registry.appState.Entities.Rooms.Build(blueprintRoom);
                blueprintRoom = null;
            }

            void RemoveBlueprintRoom()
            {
                Registry.appState.Entities.Rooms.Remove(blueprintRoom);
                blueprintRoom = null;
            }

            void ResetBlueprintRoom()
            {
                RemoveBlueprintRoom();
                CreateBlueprintRoom();
            }

            public void SetSelectedRoomCategory(string roomCategory)
            {
                selectedRoomCategory = roomCategory;
                List<RoomDefinition> roomDefinitions = Registry.Definitions.Entities.Rooms.Queries.FindByCategory(selectedRoomCategory);

                RoomDefinition roomDefinition = roomDefinitions[0];

                if (roomDefinition != null)
                {
                    SelectRoomDefinitionAndUpdateBlueprint(roomDefinition);
                }

                events.onSelectedRoomCategoryUpdated?.Invoke(roomCategory);
                events.onSelectedRoomDefinitionUpdated?.Invoke(selectedRoomDefinition);
            }

            public void SetSelectedRoomDefinition(RoomDefinition roomDefinition)
            {
                SelectRoomDefinitionAndUpdateBlueprint(roomDefinition);

                events.onSelectedRoomDefinitionUpdated?.Invoke(selectedRoomDefinition);
            }
        }
    }
}
