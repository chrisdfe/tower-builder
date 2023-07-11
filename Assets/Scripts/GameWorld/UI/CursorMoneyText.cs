using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class CursorMoneyText : MonoBehaviour
    {
        Text text;

        void Awake()
        {
            text = GetComponent<Text>();

            Registry.appState.Tools.onToolStateUpdated += OnToolStateUpdated;

            Registry.appState.Tools.Build.onBlueprintUpdated += OnBlueprintUpdated;
            Registry.appState.Tools.Build.onBuildStart += OnBuildStart;
            Registry.appState.Tools.Build.onBuildEnd += OnBuildEnd;

            Hide();
        }

        void OnDestroy()
        {
            Registry.appState.Tools.onToolStateUpdated -= OnToolStateUpdated;
            Registry.appState.Tools.Build.onBlueprintUpdated -= OnBlueprintUpdated;
            Registry.appState.Tools.Build.onBuildStart -= OnBuildStart;
            Registry.appState.Tools.Build.onBuildEnd -= OnBuildEnd;
        }

        void OnToolStateUpdated(ApplicationState.Tools.State.Key newToolState, ApplicationState.Tools.State.Key previousToolState)
        {
            if (previousToolState == ApplicationState.Tools.State.Key.Build)
            {
                Hide();
            }
        }

        void OnBlueprintUpdated(EntityGroup blueprint)
        {
            if (Registry.appState.Tools.currentKey == ApplicationState.Tools.State.Key.Build)
            {
                SetPosition();
                SetText();
            }
        }

        void OnBuildStart()
        {
            Show();
        }

        void OnBuildEnd()
        {
            // TODO - play animation
            Hide();
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        void SetPosition()
        {
            CellCoordinates currentSelectedCell = Registry.appState.UI.selectedCell;
            Vector3 worldPosition = GameWorldUtils.CellCoordinatesToPosition(currentSelectedCell);
            Vector3 adjustedWorldPosition = new Vector3(
                worldPosition.x,
                worldPosition.y + Entities.Constants.CELL_HEIGHT / 4,
                0
            );

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(adjustedWorldPosition);

            transform.position = new Vector3(
                screenPosition.x,
                screenPosition.y,
                0
            );
        }

        void SetText()
        {
            EntityGroup blueprint = Registry.appState.Tools.Build.blueprint;
            int amount = blueprint.price;
            text.text = String.Format("${0:n0}", amount);

            if (blueprint.buildValidator.isValid)
            {
                text.color = Color.white;
            }
            else
            {
                text.color = Color.red;
            }
        }
    }
}