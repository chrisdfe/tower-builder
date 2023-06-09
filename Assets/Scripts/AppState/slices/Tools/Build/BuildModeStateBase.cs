using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.ApplicationState.Tools
{
    public abstract class BuildModeStateBase : ToolStateBase
    {
        protected Build.State buildState;
        protected bool isLocked;

        public BuildModeStateBase(AppState appState, Tools.State toolsState, Build.State buildState) : base(appState, toolsState)
        {
            this.buildState = buildState;
        }

        public virtual void OnBuildStart() { }

        public virtual void OnBuildEnd() { }
    }
}