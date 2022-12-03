using System;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class NoneToolStateInputHandlers : ToolStateInputHandlersBase
    {
        public NoneToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

        public override void OnMouseDown()
        {
            base.OnMouseDown();
        }
    }
}