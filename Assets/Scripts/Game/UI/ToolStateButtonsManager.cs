using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class ToolStateButtonsManager : MonoBehaviour
    {
        Button NoneButton;
        Button BuildButton;
        Button CondoButton;

        void Awake()
        {
            NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            BuildButton = transform.Find("BuildButton").GetComponent<Button>();

            NoneButton.onClick.AddListener(OnNoneButtonClick);
            BuildButton.onClick.AddListener(OnBuildButtonClick);
        }

        void OnNoneButtonClick()
        {
            OnToolButtonClick(ToolState.None);
        }

        void OnBuildButtonClick()
        {
            OnToolButtonClick(ToolState.Build);
        }

        void OnToolButtonClick(ToolState toolState)
        {
            Registry.Stores.MapUI.SetToolState(toolState);
        }

    }
}