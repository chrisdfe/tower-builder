using System;
using TowerBuilder;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class ToolStateHandlersBase
    {
        protected MapManager mapManager;

        public ToolStateHandlersBase(MapManager parentMapManager)
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