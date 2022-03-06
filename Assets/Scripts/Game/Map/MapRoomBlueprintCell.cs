using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.UI;
using UnityEngine;

public class MapRoomBlueprintCell : MonoBehaviour
{
    static Color VALID_COLOR = new Color(0, 0, 255, 0.5f);
    static Color INVALID_COLOR = new Color(255, 0, 0, 0.5f);

    // public MapRoomBlueprint parentMapRoomBlueprint;

    RoomBlueprintCell roomBlueprintCell;

    Renderer _renderer;

    public void SetRoomBlueprintCell(RoomBlueprintCell roomBlueprintCell)
    {
        this.roomBlueprintCell = roomBlueprintCell;
        transform.localPosition = MapCellHelpers.CellCoordinatesToPosition(roomBlueprintCell.absoluteCellCoordinates);
        UpdateMaterialColor();
    }

    public void UpdateMaterialColor()
    {
        if (IsValid())
        {
            _renderer.material.color = VALID_COLOR;
        }
        else
        {
            _renderer.material.color = INVALID_COLOR;
        }
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    bool IsValid()
    {
        if (!roomBlueprintCell.parentBlueprint.IsValid())
        {
            return false;
        }

        if (!roomBlueprintCell.IsValid())
        {
            return false;
        }

        return true;
    }
}
