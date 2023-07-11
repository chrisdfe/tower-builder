using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;

namespace TowerBuilder.ApplicationState.Tools
{
    public abstract class BuildModeStateBase : ISetupable
    {
        protected bool isLocked;

        public delegate void ResetEvent();
        public ResetEvent onResetRequested;

        protected AppState appState;

        public BuildModeStateBase(AppState appState)
        {
            this.appState = appState;
        }

        public virtual void Setup() { }
        public virtual void Teardown() { }

        public virtual EntityGroup CalculateBlueprintEntityGroup() => new EntityGroup();
    }
}