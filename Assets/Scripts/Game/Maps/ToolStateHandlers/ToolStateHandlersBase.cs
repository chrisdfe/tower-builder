using System;
using TowerBuilder;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class ToolStateHandlersBase
    {
        protected GameMapManager mapManager;

        public ToolStateHandlersBase(GameMapManager parentMapManager)
        {
            mapManager = parentMapManager;
        }

        public virtual void Update() { }

        public virtual void OnTransitionFrom(ToolState nextToolState) { }

        public virtual void OnTransitionTo(ToolState previousToolState) { }

        public virtual void OnMouseDown() { }

        public virtual void OnMouseUp() { }
    }
}