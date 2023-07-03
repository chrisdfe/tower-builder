using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.ApplicationState.Tools
{
    public abstract class BuildModeStateBase : ToolStateBase
    {
        protected bool isLocked;

        public BuildModeStateBase(AppState appState) : base(appState) { }

        public virtual void OnBuildStart() { }

        public virtual void OnBuildEnd() { }
    }
}