using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;

public class GameWorldGroundCell : MonoBehaviour
{
    public bool isVisible { get; private set; } = true;
    public CellCoordinates cellCoordinates { get; private set; }

    Transform groundCellMesh;
    Transform wrapper;
    Transform frontWall;
    Transform frontWallFull;

    public void SetCoordinates(CellCoordinates cellCoordinates)
    {
        this.cellCoordinates = cellCoordinates;
        transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates);
    }

    public void SetVisible(bool isVisible)
    {
        this.isVisible = isVisible;
        frontWallFull.GetComponent<MeshRenderer>().enabled = isVisible;
    }

    void Awake()
    {
        groundCellMesh = transform.Find("GroundCellMesh");
        wrapper = groundCellMesh.Find("Wrapper");
        frontWall = wrapper.Find("FrontWall");
        frontWallFull = frontWall.Find("FrontWallFull");
    }
}
