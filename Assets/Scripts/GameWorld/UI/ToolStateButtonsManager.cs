using System.Collections;
using System.Collections.Generic;
using TowerBuilder.State;
using TowerBuilder.State.Tools;
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
        Button RoutesButton;

        Button currentButton;
        Color originalColor;

        void Awake()
        {
            // NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            BuildButton = transform.Find("BuildButton").GetComponent<Button>();
            DestroyButton = transform.Find("DestroyButton").GetComponent<Button>();
            InspectButton = transform.Find("InspectButton").GetComponent<Button>();
            RoutesButton = transform.Find("RoutesButton").GetComponent<Button>();

            // NoneButton.onClick.AddListener(OnNoneButtonClick);
            BuildButton.onClick.AddListener(OnBuildButtonClick);
            DestroyButton.onClick.AddListener(OnDestroyButtonClick);
            InspectButton.onClick.AddListener(OnInspectButtonClick);
            RoutesButton.onClick.AddListener(OnRoutesButtonClick);

            new List<Button> { BuildButton, DestroyButton, InspectButton, RoutesButton }.ForEach(button =>
            {
                button.image.color = DEFAULT_BG_COLOR;
                button.transform.Find("Text").GetComponent<Text>().color = DEFAULT_TEXT_COLOR;
            });

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
        }

        // void OnNoneButtonClick()
        // {
        //     OnToolButtonClick(ToolState.None);
        // }

        void OnBuildButtonClick()
        {
            OnToolButtonClick(ToolState.Build);
        }

        void OnDestroyButtonClick()
        {
            OnToolButtonClick(ToolState.Destroy);
        }

        void OnInspectButtonClick()
        {
            OnToolButtonClick(ToolState.Inspect);
        }

        void OnRoutesButtonClick()
        {
            OnToolButtonClick(ToolState.Routes);
        }

        void OnToolButtonClick(ToolState toolState)
        {
            ToolState currentToolState = Registry.appState.Tools.toolState;
            ToolState newToolState = (currentToolState == toolState) ? ToolState.None : toolState;
            Registry.appState.Tools.SetToolState(newToolState);
        }

        void OnToolStateUpdated(ToolState toolState, ToolState previousToolState)
        {
            if (currentButton != null)
            {
                currentButton.image.color = DEFAULT_BG_COLOR;
                currentButton.transform.Find("Text").GetComponent<Text>().color = DEFAULT_TEXT_COLOR;
            }

            currentButton = GetToolStateButton(toolState);

            if (currentButton != null)
            {
                currentButton.image.color = PRESSED_BG_COLOR;
                currentButton.transform.Find("Text").GetComponent<Text>().color = PRESSED_TEXT_COLOR;
            }
        }

        Button GetToolStateButton(ToolState toolState)
        {
            if (toolState == ToolState.Build)
            {
                return BuildButton;
            }

            if (toolState == ToolState.Destroy)
            {
                return DestroyButton;
            }

            if (toolState == ToolState.Inspect)
            {
                return InspectButton;
            }

            if (toolState == ToolState.Routes)
            {
                return RoutesButton;
            }

            // return NoneButton;
            return null;
        }
    }
}