using System;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class NoneToolStateHandlers : ToolStateHandlersBase
    {
        public NoneToolStateHandlers(MapManager parentMapManager) : base(parentMapManager) { }

        public override void OnMouseDown()
        {
            Debug.Log("ToolState.None mouseDown");
        }
    }
}