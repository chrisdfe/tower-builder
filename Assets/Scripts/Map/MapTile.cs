using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public int x;
    public int y;

    // private Collider coll;

    public delegate void MapTileMouseEvent(MapTile mapTile);
    public MapTileMouseEvent onMouseOver;
    public MapTileMouseEvent onMouseExit;
    public MapTileMouseEvent onMouseDown;
    public MapTileMouseEvent onMouseUp;

    // Start is called before the first frame update
    void Start()
    {
        // coll = GetComponent<Collider>();
    }

    void OnMouseOver()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hitInfo;

        // if (coll.Raycast(ray, out hitInfo, 100.0f))
        // {
        //     Debug.Log(hitInfo.point);
        // }

        if (onMouseOver != null)
        {
            onMouseOver(this);
        }
    }

    void OnMouseExit()
    {

        if (onMouseExit != null)
        {
            onMouseExit(this);
        }
    }

    void OnMouseDown()
    {
        if (onMouseDown != null)
        {
            onMouseDown(this);
        }
    }

    void OnMouseUp()
    {
        if (onMouseUp != null)
        {
            onMouseUp(this);
        }
    }
}
