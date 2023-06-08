using System;
using TowerBuilder;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
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

        public virtual void OnTransitionFrom(ApplicationState.Tools.State.Key nextToolState) { }

        public virtual void OnTransitionTo(ApplicationState.Tools.State.Key previousToolState) { }

        public virtual void OnMouseDown() { }

        public virtual void OnMouseUp() { }
    }
}