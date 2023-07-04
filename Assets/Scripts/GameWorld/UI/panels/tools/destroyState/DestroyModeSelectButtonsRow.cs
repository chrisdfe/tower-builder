using System.Collections.Generic;
using TowerBuilder.GameWorld.UI.Components;
using UnityEngine;

namespace TowerBuilder.GameWorld.UI
{
    public class DestroyModeSelectButtonsRow : UIButtonsRowBase
    {
        public DestroyModeSelectButtonsRow() : base() { }

        public override void Setup()
        {
            base.Setup();

            Registry.appState.Tools.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.Destroy.onModeUpdated += OnDestroyModeUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            Registry.appState.Tools.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.Destroy.onModeUpdated -= OnDestroyModeUpdated;
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
            Registry.appState.Tools.Destroy.currentMode switch
            {
                ApplicationState.Tools.Destroy.State.Mode.Entities => button.value == "Entities",
                ApplicationState.Tools.Destroy.State.Mode.Rooms => button.value == "Rooms",
                _ => false
            };


        public override void OnButtonClick(string value)
        {
            switch (value)
            {
                case "Entities":
                    Registry.appState.Tools.Destroy.SetMode(ApplicationState.Tools.Destroy.State.Mode.Entities);
                    break;
                case "Rooms":
                    Registry.appState.Tools.Destroy.SetMode(ApplicationState.Tools.Destroy.State.Mode.Rooms);
                    break;
            }
        }

        /* 
            Event Handlers
        */
        void OnToolStateUpdated(ApplicationState.Tools.State.Key newKey, ApplicationState.Tools.State.Key previousKey)
        {
            if (newKey == ApplicationState.Tools.State.Key.Destroy)
            {
                HighlightSelectedButton();
            }
        }

        void OnDestroyModeUpdated(ApplicationState.Tools.Destroy.State.Mode newMode, ApplicationState.Tools.Destroy.State.Mode previousMode)
        {
            Debug.Log("OnDestroyModeUpdated");
            Debug.Log(newMode);
            HighlightSelectedButton();
        }
    }
}