using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class ToolStateButtonsManager : MonoBehaviour
    {
        // TODO - put this in a more centralized place
        static Color DEFAULT_BG_COLOR = Color.white;
        static Color DEFAULT_TEXT_COLOR = ColorUtils.ColorFromHex("#033A34");

        static Color PRESSED_BG_COLOR = ColorUtils.ColorFromHex("#033A34");
        static Color PRESSED_TEXT_COLOR = Color.white;

        // Button NoneButton;
        Button BuildButton;
        Button DestroyButton;
        Button InspectButton;

        Button currentButton;
        Color originalColor;

        List<Button> buttons;

        void Awake()
        {
            // NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            BuildButton = transform.Find("BuildButton").GetComponent<Button>();
            DestroyButton = transform.Find("DestroyButton").GetComponent<Button>();
            InspectButton = transform.Find("InspectButton").GetComponent<Button>();

            // NoneButton.onClick.AddListener(OnNoneButtonClick);
            BuildButton.onClick.AddListener(OnBuildButtonClick);
            DestroyButton.onClick.AddListener(OnDestroyButtonClick);
            InspectButton.onClick.AddListener(OnInspectButtonClick);

            currentButton = GetToolStateButton(Registry.appState.Tools.currentKey);
            buttons = new List<Button> { BuildButton, DestroyButton, InspectButton };
            HighlightCurrentButton();

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
        }

        void OnBuildButtonClick()
        {
            OnToolButtonClick(ApplicationState.Tools.State.Key.Build);
        }

        void OnDestroyButtonClick()
        {
            OnToolButtonClick(ApplicationState.Tools.State.Key.Destroy);
        }

        void OnInspectButtonClick()
        {
            OnToolButtonClick(ApplicationState.Tools.State.Key.Inspect);
        }

        void OnRoutesButtonClick()
        {
            OnToolButtonClick(ApplicationState.Tools.State.Key.Routes);
        }

        void OnToolButtonClick(ApplicationState.Tools.State.Key toolState)
        {
            ApplicationState.Tools.State.Key currentToolState = Registry.appState.Tools.currentKey;
            ApplicationState.Tools.State.Key newToolState = (currentToolState == toolState) ? ApplicationState.Tools.State.DEFAULT_TOOL_STATE : toolState;
            Registry.appState.Tools.SetToolState(newToolState);
        }

        void OnToolStateUpdated(ApplicationState.Tools.State.Key toolState, ApplicationState.Tools.State.Key previousToolState)
        {
            currentButton = GetToolStateButton(toolState);

            HighlightCurrentButton();
        }

        void HighlightCurrentButton()
        {
            buttons.ForEach(button =>
            {
                if (button == currentButton)
                {
                    HighlightButton(button);
                }
                else
                {
                    UnHighlightButton(button);
                }
            });
        }

        void UnHighlightButton(Button button)
        {
            button.image.color = DEFAULT_BG_COLOR;
            button.transform.Find("Text").GetComponent<Text>().color = DEFAULT_TEXT_COLOR;
        }

        void HighlightButton(Button button)
        {
            button.image.color = PRESSED_BG_COLOR;
            button.transform.Find("Text").GetComponent<Text>().color = PRESSED_TEXT_COLOR;
        }

        Button GetToolStateButton(ApplicationState.Tools.State.Key toolState)
        {
            if (toolState == ApplicationState.Tools.State.Key.Build)
            {
                return BuildButton;
            }

            if (toolState == ApplicationState.Tools.State.Key.Destroy)
            {
                return DestroyButton;
            }

            if (toolState == ApplicationState.Tools.State.Key.Inspect)
            {
                return InspectButton;
            }

            return null;
        }
    }
}