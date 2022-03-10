using System;
using TowerBuilder;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class ToolStateInputHandlersBase
    {
        protected GameMapManager parentMapManager;

        public ToolStateInputHandlersBase(GameMapManager parentMapManager)
        {
            this.parentMapManager = parentMapManager;
        }

        public virtual void Update() { }

        public virtual void OnTransitionFrom(ToolState nextToolState) { }

        public virtual void OnTransitionTo(ToolState previousToolState) { }

        public virtual void OnMouseDown() { }

        public virtual void OnMouseUp() { }
    }
}