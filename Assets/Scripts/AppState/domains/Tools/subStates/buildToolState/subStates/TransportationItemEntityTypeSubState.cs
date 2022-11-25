using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.TransportationItems;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class TransportationItemEntityTypeSubState : EntityTypeSubState
        {
            public string selectedCategory { get; private set; } = "";
            public TransportationItemTemplate selectedTemplate { get; private set; } = null;
            public TransportationItem blueprint { get; private set; }

            public class Events
            {
                public delegate void SelectedCategoryEvent(string selectedCategory);
                public SelectedCategoryEvent onSelectedCategoryUpdated;

                public delegate void SelectedTemplateEvent(TransportationItemTemplate selectedTemplate);
                public SelectedTemplateEvent onSelectedTemplateUpdated;

                public delegate void blueprintUpdateEvent(TransportationItem blueprint);
                public blueprintUpdateEvent onBlueprintUpdated;
            }

            public Events events;

            public TransportationItemEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState)
            {
                events = new Events();

                selectedTemplate = Registry.definitions.transportationItems.definitions[0];
            }

            public override void Setup()
            {
                base.Setup();

                CreateBlueprint();
            }

            public override void Teardown()
            {
                base.Teardown();

                DestroyBlueprint();
            }

            public override void EndBuild()
            {
                base.EndBuild();

                // blueprint.validator.Validate(Registry.appState);

                // if (blueprint.validator.isValid)
                // {
                BuildBlueprint();
                CreateBlueprint();
                // }
                // else
                // {
                //     Registry.appState.Notifications.AddNotifications(
                //         blueprint.validator.errors.Select(error => error.message).ToArray()
                //     );

                //     ResetBlueprint();
                // }
            }

            public override void OnSelectionBoxUpdated()
            {
                base.OnSelectionBoxUpdated();

                ResetBlueprint();
            }

            public void SetSelectedCategory(string Category)
            {
                selectedCategory = Category;
                List<TransportationItemTemplate> definitions = Registry.definitions.transportationItems.queries.FindByCategory(selectedCategory);

                TransportationItemTemplate definition = definitions[0];

                if (definition != null)
                {
                    SelectTemplateAndUpdateBlueprint(definition);
                }

                if (events.onSelectedCategoryUpdated != null)
                {
                    events.onSelectedCategoryUpdated(selectedCategory);
                }

                if (definition != null && events.onSelectedTemplateUpdated != null)
                {
                    events.onSelectedTemplateUpdated(definition);
                }
            }

            public void SetSelectedTemplate(TransportationItemTemplate template)
            {
                SelectTemplateAndUpdateBlueprint(template);

                if (events.onSelectedTemplateUpdated != null)
                {
                    events.onSelectedTemplateUpdated(template);
                }
            }

            void CreateBlueprint()
            {
                blueprint = new TransportationItem(selectedTemplate);
                blueprint.isInBlueprintMode = true;
                blueprint.cellCoordinates = Registry.appState.UI.currentSelectedCell;
                Registry.appState.TransportationItems.AddTransportationItem(blueprint);
            }

            void DestroyBlueprint()
            {
                Registry.appState.TransportationItems.RemoveTransportationItem(blueprint);
                blueprint = null;
            }

            void BuildBlueprint()
            {
                Registry.appState.TransportationItems.BuildTransportationItem(blueprint);
                blueprint = null;
            }

            void ResetBlueprint()
            {
                DestroyBlueprint();
                CreateBlueprint();
            }

            void SelectTemplateAndUpdateBlueprint(TransportationItemTemplate template)
            {
                this.selectedTemplate = template;
                ResetBlueprint();
            }
        }
    }
}
