using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class FurnitureEntityTypeSubState : EntityTypeSubState
        {
            public string selectedFurnitureCategory { get; private set; } = "";
            public FurnitureTemplate selectedFurnitureTemplate { get; private set; } = null;
            public Furniture blueprintFurniture { get; private set; }

            public class Events
            {
                public delegate void SelectedFurnitureCategoryEvent(string selectedFurnitureCategory);
                public SelectedFurnitureCategoryEvent onSelectedFurnitureCategoryUpdated;

                public delegate void SelectedFurnitureTemplateEvent(FurnitureTemplate selectedFurnitureTemplate);
                public SelectedFurnitureTemplateEvent onSelectedFurnitureTemplateUpdated;

                public delegate void blueprintUpdateEvent(Furniture blueprintFurniture);
                public blueprintUpdateEvent onBlueprintFurnitureUpdated;
            }

            public Events events;

            public FurnitureEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState)
            {
                events = new Events();

                selectedFurnitureTemplate = Registry.definitions.furnitures.definitions[0];
            }

            public override void Setup()
            {
                base.Setup();

                CreateBlueprintFurniture();
            }

            public override void Teardown()
            {
                base.Teardown();

                DestroyBlueprintFurniture();
            }

            public override void EndBuild()
            {
                base.EndBuild();

                blueprintFurniture.validator.Validate(Registry.appState);

                if (blueprintFurniture.validator.isValid)
                {
                    BuildBlueprintFurniture();
                    CreateBlueprintFurniture();
                }
                else
                {
                    Registry.appState.Notifications.Add(
                        new NotificationsList(
                            blueprintFurniture.validator.errors.items
                                .Select(error => new Notification(error.message))
                                .ToList()
                        )
                    );

                    ResetBlueprintFurniture();
                }
            }

            public override void OnSelectionBoxUpdated()
            {
                base.OnSelectionBoxUpdated();

                ResetBlueprintFurniture();
            }

            public void SetSelectedFurnitureCategory(string furnitureCategory)
            {
                selectedFurnitureCategory = furnitureCategory;
                List<FurnitureTemplate> furnitureDefinitions = Registry.definitions.furnitures.queries.FindByCategory(selectedFurnitureCategory);

                FurnitureTemplate furnitureDefinition = furnitureDefinitions[0];

                if (furnitureDefinition != null)
                {
                    SelectFurnitureTemplateAndUpdateBlueprint(furnitureDefinition);
                }

                if (events.onSelectedFurnitureCategoryUpdated != null)
                {
                    events.onSelectedFurnitureCategoryUpdated(selectedFurnitureCategory);
                }

                if (furnitureDefinition != null && events.onSelectedFurnitureTemplateUpdated != null)
                {
                    events.onSelectedFurnitureTemplateUpdated(furnitureDefinition);
                }
            }

            public void SetSelectedFurnitureTemplate(FurnitureTemplate furnitureTemplate)
            {
                SelectFurnitureTemplateAndUpdateBlueprint(furnitureTemplate);

                if (events.onSelectedFurnitureTemplateUpdated != null)
                {
                    events.onSelectedFurnitureTemplateUpdated(furnitureTemplate);
                }
            }

            void CreateBlueprintFurniture()
            {
                blueprintFurniture = new Furniture(selectedFurnitureTemplate);
                blueprintFurniture.isInBlueprintMode = true;
                blueprintFurniture.cellCoordinatesList.PositionAtCoordinates(Registry.appState.UI.currentSelectedCell);
                Registry.appState.Entities.Furnitures.Add(blueprintFurniture);
            }

            void DestroyBlueprintFurniture()
            {
                Registry.appState.Entities.Furnitures.Remove(blueprintFurniture);
                blueprintFurniture = null;
            }

            void BuildBlueprintFurniture()
            {
                Registry.appState.Entities.Furnitures.Build(blueprintFurniture);
                blueprintFurniture = null;
            }

            void ResetBlueprintFurniture()
            {
                DestroyBlueprintFurniture();
                CreateBlueprintFurniture();
            }

            void SelectFurnitureTemplateAndUpdateBlueprint(FurnitureTemplate furnitureTemplate)
            {
                this.selectedFurnitureTemplate = furnitureTemplate;
                ResetBlueprintFurniture();
            }
        }
    }
}
