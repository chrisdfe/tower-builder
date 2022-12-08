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
                            blueprintRoom.validator.errors
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

            void SelectRoomTemplateAndUpdateBlueprint(RoomTemplate roomTemplate)
            {
                this.selectedRoomTemplate = roomTemplate;
                ResetBlueprintRoom();
            }

            void CreateBlueprintRoom()
            {
                blueprintRoom = new Room(selectedRoomTemplate);
                blueprintRoom.isInBlueprintMode = true;
                blueprintRoom.CalculateCellsFromSelectionBox(Registry.appState.UI.selectionBox);
                blueprintRoom.validator.Validate(Registry.appState);

                Registry.appState.Rooms.Add(blueprintRoom);
            }

            void BuildBlueprintRoom()
            {
                Registry.appState.Rooms.Build(blueprintRoom);
                blueprintRoom = null;
            }

            void RemoveBlueprintRoom()
            {
                Registry.appState.Rooms.Remove(blueprintRoom);
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
                List<RoomTemplate> roomDefinitions = Registry.definitions.rooms.queries.FindByCategory(selectedRoomCategory);

                RoomTemplate roomTemplate = roomDefinitions[0];

                if (roomTemplate != null)
                {
                    SelectRoomTemplateAndUpdateBlueprint(roomTemplate);
                }

                events.onSelectedRoomCategoryUpdated?.Invoke(roomCategory);
                events.onSelectedRoomTemplateUpdated?.Invoke(selectedRoomTemplate);
            }

            public void SetSelectedRoomTemplate(RoomTemplate roomTemplate)
            {
                SelectRoomTemplateAndUpdateBlueprint(roomTemplate);

                events.onSelectedRoomTemplateUpdated?.Invoke(selectedRoomTemplate);
            }
        }
    }
}
