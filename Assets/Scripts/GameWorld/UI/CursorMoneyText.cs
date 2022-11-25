using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class CursorMoneyText : MonoBehaviour
    {
        static float halfATile = Constants.TILE_SIZE / 2f;

        Text text;

        void Awake()
        {
            text = GetComponent<Text>();

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;

            Registry.appState.Tools.buildToolState.subStates.roomEntityType.events.onBlueprintRoomUpdated += OnBlueprintRoomUpdated;
            Registry.appState.Tools.buildToolState.events.onBuildStart += OnBuildStart;
            Registry.appState.Tools.buildToolState.events.onBuildEnd += OnBuildEnd;

            Hide();
        }

        void OnDestroy()
        {
            Registry.appState.Tools.events.onToolStateUpdated -= OnToolStateUpdated;
            Registry.appState.Tools.buildToolState.subStates.roomEntityType.events.onBlueprintRoomUpdated -= OnBlueprintRoomUpdated;
            Registry.appState.Tools.buildToolState.events.onBuildStart -= OnBuildStart;
            Registry.appState.Tools.buildToolState.events.onBuildEnd -= OnBuildEnd;
        }

        void OnToolStateUpdated(ToolState newToolState, ToolState previousToolState)
        {
            if (previousToolState == ToolState.Build)
            {
                Hide();
            }
        }

        void OnBlueprintRoomUpdated(Room blueprintRoom)
        {
            if (Registry.appState.Tools.toolState == ToolState.Build)
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
            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            Vector3 worldPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(currentSelectedCell);
            Vector3 adjustedWorldPosition = new Vector3(
                worldPosition.x,
                worldPosition.y + halfATile + 0.2f,
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
            Room blueprintRoom = Registry.appState.Tools.buildToolState.subStates.roomEntityType.blueprintRoom;
            int amount = blueprintRoom.price;
            text.text = String.Format("${0:n0}", amount);

            if (blueprintRoom.validator.isValid)
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