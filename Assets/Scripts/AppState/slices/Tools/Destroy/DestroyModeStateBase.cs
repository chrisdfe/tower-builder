using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.ApplicationState.Tools
{
    public abstract class DestroyModeStateBase : ToolStateBase
    {
        protected bool isLocked;

        public DestroyModeStateBase(AppState appState) : base(appState) { }

        public virtual void OnDestroyStart() { }

        public virtual void OnDestroyEnd() { }

        public virtual ListWrapper<Entity> CalculateEntitiesToDelete() => new ListWrapper<Entity>();
    }
}