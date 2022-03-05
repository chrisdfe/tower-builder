using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.UI;
using UnityEngine;

public class MapCursorCell : MonoBehaviour
{
    static Color VALID_COLOR = new Color(0, 0, 255, 0.5f);
    static Color INVALID_COLOR = new Color(255, 0, 0, 0.5f);

    public MapCursor parentMapCursor;

    RoomBlueprintCell roomBlueprintCell;

    Renderer renderer;

    public void SetRoomBlueprintCell(RoomBlueprintCell roomBlueprintCell)
    {
        this.roomBlueprintCell = roomBlueprintCell;
        transform.localPosition = MapCellHelpers.CellCoordinatesToPosition(roomBlueprintCell.absoluteCellCoordinates);
        UpdateMaterialColor();
    }

    public void UpdateMaterialColor()
    {
        if (roomBlueprintCell.parentBlueprint.IsValid() && roomBlueprintCell.IsValid())
        {
            renderer.material.color = VALID_COLOR;
        }
        else
        {
            renderer.material.color = INVALID_COLOR;
        }
    }

    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    bool IsValid()
    {
        if (!roomBlueprintCell.IsValid())
        {
            return false;
        }

        return true;
    }
}
