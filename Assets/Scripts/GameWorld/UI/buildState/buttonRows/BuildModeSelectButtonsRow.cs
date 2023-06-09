using System.Collections.Generic;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class BuildModeSelectButtonsRow : BuildToolStateButtonsRowBase
    {
        public BuildModeSelectButtonsRow() : base() { }

        public override void Setup()
        {
            base.Setup();

            Registry.appState.Tools.Build.events.onModeUpdated += OnBuildModeUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            Registry.appState.Tools.Build.events.onModeUpdated -= OnBuildModeUpdated;
        }

        public override List<UISelectButton.Input> CreateButtonInputs() =>
            new List<UISelectButton.Input>() {
                new UISelectButton.Input() {
                    label = "Entities",
                    value = "Entities",
                },
                new UISelectButton.Input() {
                    label = "Rooms",
                    value = "Rooms",
                },
            };

        public override bool ButtonShouldBeSelected(UISelectButton button) =>
            Registry.appState.Tools.Build.currentMode switch
            {
                ApplicationState.Tools.Build.State.Mode.Entities => button.value == "Entities",
                ApplicationState.Tools.Build.State.Mode.Rooms => button.value == "Rooms",
                _ => false
            };


        public override void OnButtonClick(string value)
        {
            switch (value)
            {
                case "Entities":
                    Registry.appState.Tools.Build.SetMode(ApplicationState.Tools.Build.State.Mode.Entities);
                    break;
                case "Rooms":
                    Registry.appState.Tools.Build.SetMode(ApplicationState.Tools.Build.State.Mode.Rooms);
                    break;
            }
        }

        void OnBuildModeUpdated(ApplicationState.Tools.Build.State.Mode newMode, ApplicationState.Tools.Build.State.Mode previousMode)
        {
            HighlightSelectedButton();
        }
    }
}