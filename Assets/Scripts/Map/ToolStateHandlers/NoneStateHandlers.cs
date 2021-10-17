using System;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class NoneStateHandlers : ToolStateHandlerBase
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
        }

        void OnMouseDown() { }

        void OnMouseUp() { }
    }
}