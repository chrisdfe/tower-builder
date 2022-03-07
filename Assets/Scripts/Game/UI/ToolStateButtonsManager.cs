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
        Button DestroyButton;

        void Awake()
        {
            NoneButton = transform.Find("NoneButton").GetComponent<Button>();
            BuildButton = transform.Find("BuildButton").GetComponent<Button>();
            DestroyButton = transform.Find("DestroyButton").GetComponent<Button>();

            NoneButton.onClick.AddListener(OnNoneButtonClick);
            BuildButton.onClick.AddListener(OnBuildButtonClick);
            DestroyButton.onClick.AddListener(OnDestroyButtonClick);
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

        void OnToolButtonClick(ToolState toolState)
        {
            Registry.Stores.MapUI.SetToolState(toolState);
        }
    }
}