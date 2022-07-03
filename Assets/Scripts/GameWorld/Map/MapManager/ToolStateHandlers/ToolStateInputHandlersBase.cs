using System;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public abstract class ToolStateInputHandlersBase
    {
        protected GameWorldMapManager parentMapManager;

        public ToolStateInputHandlersBase(GameWorldMapManager parentMapManager)
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