using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class ToolStateButtonsManager : MonoBehaviour
    {
        static Color PRESSED_COLOR = Color.red;

        Button NoneButton;
        Button BuildButton;
        Button DestroyButton;
        Button InspectButton;
        Button RoutesButton;

        Button currentButton;
        Color originalColor;

        void Awake()
        {
            NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            BuildButton = transform.Find("BuildButton").GetComponent<Button>();
            DestroyButton = transform.Find("DestroyButton").GetComponent<Button>();
            InspectButton = transform.Find("InspectButton").GetComponent<Button>();
            RoutesButton = transform.Find("RoutesButton").GetComponent<Button>();

            NoneButton.onClick.AddListener(OnNoneButtonClick);
            BuildButton.onClick.AddListener(OnBuildButtonClick);
            DestroyButton.onClick.AddListener(OnDestroyButtonClick);
            InspectButton.onClick.AddListener(OnInspectButtonClick);
            RoutesButton.onClick.AddListener(OnRoutesButtonClick);

            originalColor = NoneButton.colors.normalColor;

            Registry.Stores.MapUI.onToolStateUpdated += OnToolStateUpdated;
        }

        void OnNoneButtonClick()
        {
            OnToolButtonClick(ToolState.None);
        }

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
            ToolState currentToolState = Registry.Stores.MapUI.toolState;
            ToolState newToolState = (currentToolState == toolState) ? ToolState.None : toolState;
            Registry.Stores.MapUI.SetToolState(newToolState);
        }

        void OnToolStateUpdated(ToolState toolState, ToolState previousToolState)
        {
            if (currentButton != null)
            {
                currentButton.image.color = originalColor;
            }

            currentButton = GetToolStateButton(toolState);
            currentButton.image.color = PRESSED_COLOR;
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

            return NoneButton;
        }
    }
}