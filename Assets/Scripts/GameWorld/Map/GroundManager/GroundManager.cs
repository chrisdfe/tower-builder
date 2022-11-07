using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State;
using TowerBuilder.State.Tools;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map
{
    public class GroundManager : MonoBehaviour
    {
        GameObject gameWorldGroundCellPrefab;
        GameObject groundPlaceholder;

        // TODO - put these somewhere else
        const int MAP_CELLS_WIDTH = 50;
        const int MAP_CELLS_HEIGHT = 50;
        const int GROUND_STARTING_FLOOR = -1;

        Dictionary<(int x, int floor), GameWorldGroundCell> gameWorldGroundCells = new Dictionary<(int x, int floor), GameWorldGroundCell>();

        void Awake()
        {
            gameWorldGroundCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Ground/GroundCell");
            groundPlaceholder = transform.Find("GroundPlaceholder").gameObject;

            groundPlaceholder.SetActive(false);

            Registry.appState.Rooms.events.onRoomAdded += OnRoomAdded;
            Registry.appState.Rooms.events.onRoomRemoved += OnRoomDestroyed;

            Registry.appState.UI.events.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
            Registry.appState.Tools.buildToolSubState.events.onSelectedRoomTemplateUpdated += OnSelectedRoomTemplateUpdated;
        }

        void Start()
        {
            // CreateGroundCells();
        }

        void OnDestroy()
        {
            Registry.appState.Rooms.events.onRoomAdded -= OnRoomAdded;
            Registry.appState.Rooms.events.onRoomRemoved -= OnRoomDestroyed;

            Registry.appState.UI.events.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;

            Registry.appState.Tools.events.onToolStateUpdated -= OnToolStateUpdated;
            Registry.appState.Tools.buildToolSubState.events.onSelectedRoomTemplateUpdated -= OnSelectedRoomTemplateUpdated;
        }

        void OnRoomAdded(Room room)
        {
            foreach (RoomCell roomCell in room.blocks.cells.cells)
            {
                SetGroundCellVisibility(roomCell.coordinates, false);
            }
        }

        void OnRoomDestroyed(Room room)
        {
            foreach (RoomCell roomCell in room.blocks.cells.cells)
            {
                SetGroundCellVisibility(roomCell.coordinates, true);
            }
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates) { }

        void OnToolStateUpdated(ToolState toolState, ToolState previousToolState) { }

        void OnSelectedRoomTemplateUpdated(RoomTemplate roomTemplate) { }

        void CreateGroundCells()
        {
            for (int x = -MAP_CELLS_WIDTH; x < MAP_CELLS_WIDTH; x++)
            {
                for (int floor = GROUND_STARTING_FLOOR; floor > GROUND_STARTING_FLOOR - MAP_CELLS_HEIGHT; floor--)
                {
                    CreateGroundCellAtCoordinates(new CellCoordinates(x, floor));
                }
            }
        }

        void CreateGroundCellAtCoordinates(CellCoordinates cellCoordinates)
        {
            GameObject gameWorldGroundCellGameObject = Instantiate<GameObject>(gameWorldGroundCellPrefab);
            GameWorldGroundCell groundCell = gameWorldGroundCellGameObject.GetComponent<GameWorldGroundCell>();

            groundCell.transform.parent = transform;
            groundCell.SetCoordinates(cellCoordinates);

            gameWorldGroundCells[(cellCoordinates.x, cellCoordinates.floor)] = groundCell;
        }

        void SetGroundCellsVisibility(List<CellCoordinates> cells, bool isVisible)
        {
            foreach (CellCoordinates cellCoordinates in cells)
            {
                SetGroundCellVisibility(cellCoordinates, isVisible);
            }
        }

        void SetGroundCellVisibility(CellCoordinates cellCoordinates, bool isVisible)
        {
            if (gameWorldGroundCells.ContainsKey((cellCoordinates.x, cellCoordinates.floor)))
            {
                GameWorldGroundCell groundCell = gameWorldGroundCells[(cellCoordinates.x, cellCoordinates.floor)];
                groundCell.SetVisible(isVisible);
            }
        }
    }
}