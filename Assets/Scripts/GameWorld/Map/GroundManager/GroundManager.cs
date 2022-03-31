using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Blueprints;
using TowerBuilder.Stores.UI;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    GameObject gameWorldGroundCellPrefab;
    GameObject groundPlaceholder;
    List<CellCoordinates> currentBlueprintCells = new List<CellCoordinates>();

    // TODO - put these somewhere else
    const int MAP_CELLS_WIDTH = 50;
    const int MAP_CELLS_HEIGHT = 50;
    const int GROUND_STARTING_FLOOR = -1;


    // List<GameWorldGroundCell> gameWorldGroundCells = new List<GameWorldGroundCell>();
    Dictionary<(int x, int floor), GameWorldGroundCell> gameWorldGroundCells = new Dictionary<(int x, int floor), GameWorldGroundCell>();

    void Awake()
    {
        gameWorldGroundCellPrefab = Resources.Load<GameObject>("Prefabs/Map/Ground/GroundCell");
        groundPlaceholder = transform.Find("GroundPlaceholder").gameObject;

        groundPlaceholder.SetActive(false);

        Registry.Stores.Rooms.onRoomAdded += OnRoomAdded;
        Registry.Stores.Rooms.onRoomDestroyed += OnRoomDestroyed;
        Registry.Stores.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
        Registry.Stores.UI.onToolStateUpdated += OnToolStateUpdated;
        Registry.Stores.UI.buildToolSubState.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
    }

    void Start()
    {
        CreateGroundCells();
    }

    void OnRoomAdded(Room room)
    {
        foreach (RoomCell roomCell in room.roomCells.cells)
        {
            SetGroundCellVisibility(roomCell.coordinates, false);
        }
    }

    void OnRoomDestroyed(Room room)
    {
        foreach (RoomCell roomCell in room.roomCells.cells)
        {
            SetGroundCellVisibility(roomCell.coordinates, true);
        }
    }

    void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
    {
        if (Registry.Stores.UI.toolState != ToolState.Build)
        {
            return;
        }

        UpdateBlueprintGroundCellsVisibility();
    }

    void OnToolStateUpdated(ToolState toolState, ToolState previousToolState)
    {
        if (toolState == ToolState.Build)
        {
            SetCurrentBlueprintCells();
            SetBlueprintGroundCellsVisibility(false);
            SetGroundCellsVisibility(currentBlueprintCells, false);
        }
        else if (previousToolState == ToolState.Build)
        {
            SetBlueprintGroundCellsVisibility(true);
            currentBlueprintCells = new List<CellCoordinates>();
        }
    }

    void OnSelectedRoomKeyUpdated(RoomKey roomKey)
    {
        UpdateBlueprintGroundCellsVisibility();
    }

    void SetCurrentBlueprintCells()
    {
        Blueprint currentBlueprint = Registry.Stores.UI.buildToolSubState.currentBlueprint;
        currentBlueprintCells = currentBlueprint.room.roomCells.cells.Select(roomCell => roomCell.coordinates).ToList();
    }

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

    void UpdateBlueprintGroundCellsVisibility()
    {
        SetBlueprintGroundCellsVisibility(true);
        SetCurrentBlueprintCells();
        SetBlueprintGroundCellsVisibility(false);
    }

    void SetBlueprintGroundCellsVisibility(bool isVisible)
    {
        foreach (CellCoordinates cellCoordinates in currentBlueprintCells)
        {
            bool cellIsOccupied = Registry.Stores.Rooms.rooms.FindRoomAtCell(cellCoordinates) != null;
            if (
                gameWorldGroundCells.ContainsKey((cellCoordinates.x, cellCoordinates.floor)) &&
                !cellIsOccupied
            )
            {
                SetGroundCellVisibility(cellCoordinates, isVisible);
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
